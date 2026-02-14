using Core.MVP;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowFactory : IWindowFactory
    {
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.MainMenuWindow;

        public MainMenuWindowFactory(
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings)
        {
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            //TODO: Get MainMenuWindowModel from ModelProvider
            var model = new MainMenuWindowModel(0);
            var mainMenuWindowView = await _windowViewProvider.GetAsync<IMainMenuWindowView>(ViewName, WindowType.Main);
            var mainMenuWindowPresenter = new MainMenuWindowPresenter();
            mainMenuWindowPresenter.Init(mainMenuWindowView, model);

            return mainMenuWindowPresenter;
        }
    }
}