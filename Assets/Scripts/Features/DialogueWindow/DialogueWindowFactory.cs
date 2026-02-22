using Core.ModelProvider;
using Core.MVP;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Features.ChoiceWindow;
using Features.Narrative;
using Settings;
using ViewInterfaces;

namespace Features.DialogueWindow
{
    public class DialogueWindowFactory : IWindowFactory
    {
        private readonly IWindowManager _windowManager;
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly IModelProvider _modelProvider;
        private readonly NarrativeController _narrativeController;
        private readonly NarrativeModel _narrativeModel;
        private readonly ChoiceWindowModel _choiceWindowModel;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.DialogueWindow;

        public DialogueWindowFactory(
            IWindowManager windowManager,
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings,
            IModelProvider modelProvider,
            NarrativeController narrativeController,
            NarrativeModel narrativeModel,
            ChoiceWindowModel choiceWindowModel)
        {
            _windowManager = windowManager;
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
            _modelProvider = modelProvider;
            _narrativeController = narrativeController;
            _narrativeModel = narrativeModel;
            _choiceWindowModel = choiceWindowModel;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            var model = new DialogueWindowModel(_narrativeModel, _modelProvider.GetUniqueId());
            var view = await _windowViewProvider.GetAsync<IDialogueWindowView>(ViewName, WindowType.Main);
            var presenter = new DialogueWindowPresenter(
                _windowManager, 
                _localSettings,
                _narrativeController,
                _choiceWindowModel);

            presenter.Init(view, model);

            return presenter;
        }
    }
}