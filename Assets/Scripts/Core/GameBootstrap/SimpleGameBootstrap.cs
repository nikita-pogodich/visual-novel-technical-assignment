using System.Collections.Generic;
using Core.ModelProvider;
using Core.SaveSystem;
using Cysharp.Threading.Tasks;
using Features.ChoiceWindow;
using Features.DialogueWindow;
using Features.EndingScreenWindow;
using Features.MainMenu;
using Features.Narrative;
using Features.SaveLoad;
using Settings;
using UnityEngine;
using ViewInterfaces;

namespace Core.GameBootstrap
{
    public class SimpleGameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private LocalSettings _localSettings;

        [SerializeField]
        private TextAsset _story;

        private readonly WindowManager.WindowManager _windowManager = new();
        private readonly IModelProvider _modelProvider = new SimpleModelProvider();
        private readonly ResourcesManager.ResourcesManager _resourcesManager = new();
        private NarrativeController _narrativeController;
        private ISaveSystem _saveSystem;

        private void Start()
        {
            InitAsync().Forget();
        }

        private void OnDestroy()
        {
            _windowManager.Dispose();
            _narrativeController.Deinit();
        }

        private async UniTaskVoid InitAsync()
        {
            await _resourcesManager.InitializeAsync(destroyCancellationToken);

            InitSaveSystem();

            var viewProvider = new ViewProvider.ViewProvider(_resourcesManager);
            var windowViewProvider = new WindowViewProvider.WindowViewProvider(
                _localSettings,
                _resourcesManager,
                viewProvider);

            await windowViewProvider.InitializeAsync(destroyCancellationToken);

            List<CharacterData> allCharactersData = new();
            foreach (CharacterSetting characterSetting in _localSettings.CharacterSettings.Characters)
            {
                allCharactersData.Add(
                    new CharacterData(
                        characterSetting.CharacterId,
                        characterSetting.DisplayName,
                        characterSetting.InkEntryPath));
            }

            var narrativeEngine = new InkNarrativeEngine();
            var narrativeModel = new NarrativeModel(allCharactersData, narrativeEngine);

            var saveLoadManager = new SaveLoadManager(narrativeModel, narrativeEngine, _saveSystem);

            _narrativeController = new NarrativeController(
                viewProvider,
                _windowManager,
                _localSettings,
                _resourcesManager,
                saveLoadManager,
                narrativeModel,
                _story.text);

            _narrativeController.Init();

            var mainMenuWindowFactory = new MainMenuWindowFactory(
                _narrativeController,
                windowViewProvider,
                _modelProvider,
                _localSettings,
                _saveSystem);

            _windowManager.RegisterWindowFactory(mainMenuWindowFactory);

            var choiceWindowModel = new ChoiceWindowModel(_modelProvider, narrativeModel, _modelProvider.GetUniqueId());
            var choiceWindowFactory = new ChoiceWindowFactory(
                windowViewProvider,
                viewProvider,
                _localSettings,
                choiceWindowModel);

            _windowManager.RegisterWindowFactory(choiceWindowFactory);

            var dialogueWindowFactory = new DialogueWindowFactory(
                _windowManager,
                windowViewProvider,
                _localSettings,
                _modelProvider,
                _narrativeController,
                narrativeModel,
                choiceWindowModel);

            _windowManager.RegisterWindowFactory(dialogueWindowFactory);

            var endingWindowFactory = new EndingWindowFactory(
                _windowManager,
                windowViewProvider,
                _localSettings,
                _modelProvider);

            _windowManager.RegisterWindowFactory(endingWindowFactory);

            ShowStartWindowAsync().Forget();
        }

        private void InitSaveSystem()
        {
            string savePath = System.IO.Path.Combine(
                Application.persistentDataPath,
                _localSettings.GameSettings.SavesFolderName);

            _saveSystem = new JsonSlotSaveSystem(savePath, new NewtonsoftJsonSerializer());
        }


        private async UniTask ShowStartWindowAsync()
        {
            await _windowManager.ShowWindowAsync<IMainMenuWindowView, MainMenuWindowModel>(
                _localSettings.ViewNames.MainMenuWindow);
        }
    }
}