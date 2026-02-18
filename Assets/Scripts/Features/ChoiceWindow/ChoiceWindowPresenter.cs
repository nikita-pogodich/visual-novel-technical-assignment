using Core.MVPImplementation;
using Core.ViewProvider;
using Core.WindowManager;
using Features.ChoiceWindow.ChoicesList;
using Features.Narrative;
using R3;
using Settings;
using ViewInterfaces;

namespace Features.ChoiceWindow
{
    public class ChoiceWindowPresenter : BaseWindowPresenter<IChoiceWindowView, ChoiceWindowModel>
    {
        private readonly IViewProvider _viewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IWindowManager _windowManager;
        private readonly NarrativeController _narrativeController;
        private ChoicesListPresenter _choicesListPresenter;

        public ChoiceWindowPresenter(IViewProvider viewProvider, ILocalSettings localSettings)
        {
            _viewProvider = viewProvider;
            _localSettings = localSettings;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            Model.ChoicesListModel.Chose.Subscribe(OnChose).AddTo(ref disposableBuilder);
            _choicesListPresenter = new ChoicesListPresenter(_viewProvider, _localSettings);
            _choicesListPresenter.Init(View.ChoicesListView, Model.ChoicesListModel);
            AddChildPresenter(_choicesListPresenter);
        }

        protected override void OnShow()
        {
            _choicesListPresenter.UpdateChoicesList();
        }

        private void OnChose(Unit _)
        {
            SetShown(false);
        }
    }
}