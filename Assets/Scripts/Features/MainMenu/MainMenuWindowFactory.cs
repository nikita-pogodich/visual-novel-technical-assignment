using Core.ModelProvider;
using Core.MVP;
using Core.SaveSystem;
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
        private readonly ISaveSystem _saveSystem;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.MainMenuWindow;

        public MainMenuWindowFactory(
            NarrativeController narrativeController,
            IWindowViewProvider windowViewProvider,
            IModelProvider modelProvider,
            ILocalSettings localSettings,
            ISaveSystem saveSystem)
        {
            _narrativeController = narrativeController;
            _windowViewProvider = windowViewProvider;
            _modelProvider = modelProvider;
            _localSettings = localSettings;
            _saveSystem = saveSystem;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = new MainMenuWindowModel(_modelProvider.GetUniqueId());
            var view = await _windowViewProvider.GetAsync<IMainMenuWindowView>(ViewName, WindowType.Main);
            var presenter = new MainMenuWindowPresenter(_localSettings, _saveSystem, _narrativeController);
            presenter.Init(view, model);

            return presenter;
        }
    }
}