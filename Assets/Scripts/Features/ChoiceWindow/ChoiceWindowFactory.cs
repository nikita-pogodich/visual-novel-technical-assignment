using Core.ModelProvider;
using Core.MVP;
using Core.ViewProvider;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Features.Narrative;
using Settings;
using ViewInterfaces;

namespace Features.ChoiceWindow
{
    public class ChoiceWindowFactory : IWindowFactory
    {
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly IViewProvider _viewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly ChoiceWindowModel _choiceWindowModel;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.ChoiceWindow;

        public ChoiceWindowFactory(
            IWindowViewProvider windowViewProvider,
            IViewProvider viewProvider,
            ILocalSettings localSettings,
            ChoiceWindowModel choiceWindowModel)
        {
            _windowViewProvider = windowViewProvider;
            _viewProvider = viewProvider;
            _localSettings = localSettings;
            _choiceWindowModel = choiceWindowModel;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var view = await _windowViewProvider.GetAsync<IChoiceWindowView>(ViewName, WindowType.Popup);
            var presenter = new ChoiceWindowPresenter(_viewProvider, _localSettings);

            presenter.Init(view, _choiceWindowModel);

            return presenter;
        }
    }
}