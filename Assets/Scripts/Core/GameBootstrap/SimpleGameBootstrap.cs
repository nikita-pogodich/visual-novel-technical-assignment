using Core.ResourcesManager;
using Core.WindowManager;
using Core.WorldObjectManager;
using Cysharp.Threading.Tasks;
using Features.DialogueWindow;
using Features.MainMenu;
using Settings;
using UnityEngine;
using ViewInterfaces;

namespace Core.GameBootstrap
{
    public class SimpleGameBootstrap : MonoBehaviour
    {
        [SerializeField]
        private LocalSettings _localSettings;

        private readonly IWindowManager _windowManager = new WindowManager.WindowManager();
        private readonly IResourcesManager _resourcesManager = new ResourcesManager.ResourcesManager();
        private readonly IWorldObjectManager _worldObjectManager = new WorldObjectManager.WorldObjectManager();

        private void Start()
        {
            var viewProvider = new ViewProvider.ViewProvider(_resourcesManager);
            var windowViewProvider = new WindowViewProvider.WindowViewProvider(
                _localSettings,
                _resourcesManager,
                viewProvider);

            var mainMenuWindowFactory = new MainMenuWindowFactory(windowViewProvider, _localSettings);
            _windowManager.RegisterWindowFactory(mainMenuWindowFactory);

            var dialogueWindowFactory = new DialogueWindowFactory(windowViewProvider, _localSettings);
            _windowManager.RegisterWindowFactory(dialogueWindowFactory);

            ShowStartWindowAsync().Forget();
        }

        private async UniTask ShowStartWindowAsync()
        {
            await _windowManager.ShowWindowAsync<IMainMenuWindowView, MainMenuWindowModel>(
                _localSettings.ViewNames.MainMenuWindow);
        }
    }
}