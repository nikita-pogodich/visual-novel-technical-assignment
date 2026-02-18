using Core.ModelProvider;
using Core.MVP;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Features.Narrative;
using Settings;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowFactory : IWindowFactory
    {
        private readonly NarrativeController _narrativeController;
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly IModelProvider _modelProvider;
        private readonly ILocalSettings _localSettings;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.MainMenuWindow;

        public MainMenuWindowFactory(
            NarrativeController narrativeController,
            IWindowViewProvider windowViewProvider,
            IModelProvider modelProvider,
            ILocalSettings localSettings)
        {
            _narrativeController = narrativeController;
            _windowViewProvider = windowViewProvider;
            _modelProvider = modelProvider;
            _localSettings = localSettings;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = new MainMenuWindowModel(_modelProvider.GetUniqueId());
            var view = await _windowViewProvider.GetAsync<IMainMenuWindowView>(ViewName, WindowType.Main);
            var presenter = new MainMenuWindowPresenter(_narrativeController);
            presenter.Init(view, model);

            return presenter;
        }
    }
}