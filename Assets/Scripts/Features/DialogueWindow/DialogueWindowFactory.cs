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
        private readonly NarrativeController _narrativeController;
        private readonly NarrativeModel _narrativeModel;
        private readonly ChoiceWindowModel _choiceWindowModel;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.DialogueWindow;

        public DialogueWindowFactory(
            IWindowManager windowManager,
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings,
            NarrativeController narrativeController,
            NarrativeModel narrativeModel,
            ChoiceWindowModel choiceWindowModel)
        {
            _windowManager = windowManager;
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
            _narrativeController = narrativeController;
            _narrativeModel = narrativeModel;
            _choiceWindowModel = choiceWindowModel;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            //TODO: Get MainMenuWindowModel from ModelProvider
            var model = new DialogueWindowModel(_narrativeModel, 0);
            var dialogueWindowView = await _windowViewProvider.GetAsync<IDialogueWindowView>(ViewName, WindowType.Main);
            var dialogueWindowPresenter = new DialogueWindowPresenter(
                _windowManager, 
                _localSettings,
                _narrativeController,
                _choiceWindowModel);

            dialogueWindowPresenter.Init(dialogueWindowView, model);

            return dialogueWindowPresenter;
        }
    }
}