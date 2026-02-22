using Core.ModelProvider;
using Core.MVP;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using ViewInterfaces;

namespace Features.EndingScreenWindow
{
    public class EndingWindowFactory : IWindowFactory
    {
        private readonly IWindowManager _windowManager;
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IModelProvider _modelProvider;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.EndingWindow;

        public EndingWindowFactory(
            IWindowManager windowManager,
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings,
            IModelProvider modelProvider)
        {
            _modelProvider = modelProvider;
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
            _windowManager = windowManager;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = new EndingWindowModel(_modelProvider.GetUniqueId());
            var view = await _windowViewProvider.GetAsync<IEndingWindowView>(ViewName, WindowType.Main);
            var presenter = new EndingWindowPresenter(_windowManager, _localSettings);
            presenter.Init(view, model);

            return presenter;
        }
    }
}