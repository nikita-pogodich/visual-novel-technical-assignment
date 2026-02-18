using System;
using System.Collections.Generic;
using Core.ResourcesManager;
using Core.ViewProvider;
using Core.WindowManager;
using Features.DialogueWindow;
using Features.SaveLoad;
using R3;
using Settings;
using UnityEngine.InputSystem;
using ViewInterfaces;

namespace Features.Narrative
{
    public class NarrativeController
    {
        private const string DefaultSaveSlotId = "0";

        private readonly IViewProvider _viewProvider;
        private readonly IWindowManager _windowManager;
        private readonly ILocalSettings _localSettings;
        private readonly NarrativeModel _narrativeModel;
        private readonly IResourcesManager _resourcesManager;
        private readonly SaveLoadManager _saveLoadManager;
        private readonly string _storyJson;

        private readonly List<ICharacterView> _characterViews = new();

        private ICharacterSelectionView _characterSelectionView;
        private IDisposable _reactiveDisposable;

        private InputAction _quickSaveInputAction;
        private InputAction _quickLoadInputAction;

        public NarrativeController(
            IViewProvider viewProvider,
            IWindowManager windowManager,
            ILocalSettings localSettings,
            IResourcesManager resourcesManager,
            SaveLoadManager saveLoadManager,
            NarrativeModel narrativeModel,
            string storyJson)
        {
            _resourcesManager = resourcesManager;
            _viewProvider = viewProvider;
            _windowManager = windowManager;
            _localSettings = localSettings;
            _saveLoadManager = saveLoadManager;
            _narrativeModel = narrativeModel;
            _storyJson = storyJson;
        }

        public void Init()
        {
            DisposableBuilder disposableBuilder = Disposable.CreateBuilder();

            _characterSelectionView =
                _viewProvider.Get<ICharacterSelectionView>(_localSettings.ViewNames.CharacterSelectionView);

            _characterSelectionView.Selected.Subscribe(OnCharacterSelected).AddTo(ref disposableBuilder);

            _reactiveDisposable = disposableBuilder.Build();

            _quickSaveInputAction = InputSystem.actions.FindAction("QuickSave");
            _quickLoadInputAction = InputSystem.actions.FindAction("QuickLoad");

            _quickSaveInputAction.performed += OnQuickSave;
            _quickLoadInputAction.performed += OnQuickLoad;
        }

        public void Deinit()
        {
            _reactiveDisposable.Dispose();
            ClearCharacters();

            _quickSaveInputAction.performed -= OnQuickSave;
            _quickLoadInputAction.performed -= OnQuickLoad;
        }

        public void StartNewGame()
        {
            _narrativeModel.StartNewStory(_storyJson);
            StartGame();
        }

        public void LoadGame()
        {
            bool isLoadedSuccessful = _saveLoadManager.Load(DefaultSaveSlotId);
            if (isLoadedSuccessful)
            {
                StartGame();
            }
            else
            {
                //TODO: Show error popup
            }
        }

        public void EnableCharacterSelection(DialogueSnapshot dialogueSnapshot)
        {
            foreach (CharacterData dialogueSnapshotTalkTarget in dialogueSnapshot.TalkTargetsById.Values)
            {
                var characterView = _viewProvider.Get<ICharacterView>(_localSettings.ViewNames.CharacterView);
                _characterSelectionView.AddCharacter(characterView);

                CharacterSetting resultCharacterSetting = null;
                //TODO: preprocess CharacterSettings into dictionary
                foreach (CharacterSetting characterSetting in _localSettings.CharacterSettings.Characters)
                {
                    if (characterSetting.CharacterId == dialogueSnapshotTalkTarget.Id)
                    {
                        resultCharacterSetting = characterSetting;
                    }
                }

                if (resultCharacterSetting == null)
                {
                    continue;
                }

                characterView.InjectDependencies(_resourcesManager);

                //TODO: Add character model and store CharacterId there.Add characters presenter and process selection there
                characterView.UpdateView(
                    resultCharacterSetting.CharacterId,
                    resultCharacterSetting.CharacterVisualKey,
                    resultCharacterSetting.Position,
                    resultCharacterSetting.Rotation);

                _characterViews.Add(characterView);
            }
        }

        private void OnQuickSave(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.phase == InputActionPhase.Performed)
            {
                _saveLoadManager.Save(DefaultSaveSlotId);
            }
        }

        private void OnQuickLoad(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.phase == InputActionPhase.Performed)
            {
                ClearCharacters();
                LoadGame();
            }
        }

        private void StartGame()
        {
            _windowManager.HideAllWindows();

            DialogueSnapshot dialogueSnapshot = _narrativeModel.GetSnapshot();
            if (dialogueSnapshot.Mode == WorldMode.CharacterSelect)
            {
                EnableCharacterSelection(dialogueSnapshot);
            }
            else
            {
                ShowDialogueWindow();
            }
        }

        private void ClearCharacters()
        {
            foreach (ICharacterView characterView in _characterViews)
            {
                _viewProvider.Release(_localSettings.ViewNames.CharacterView, characterView);
                characterView.Deinit();
            }

            _characterViews.Clear();
        }

        private void OnCharacterSelected(ICharacterView characterView)
        {
            ClearCharacters();

            _narrativeModel.StartConversation(characterView.CharacterId);
            ShowDialogueWindow();
        }

        private void ShowDialogueWindow()
        {
            _windowManager.ShowWindowAsync<IDialogueWindowView, DialogueWindowModel>(
                _localSettings.ViewNames.DialogueWindow);
        }
    }
}