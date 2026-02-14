using Core.MVP;
using Core.WindowManager;
using Core.WindowViewProvider;
using Cysharp.Threading.Tasks;
using Settings;
using ViewInterfaces;

namespace Features.DialogueWindow
{
    public class DialogueWindowFactory : IWindowFactory
    {
        private readonly IWindowViewProvider _windowViewProvider;
        private readonly ILocalSettings _localSettings;

        public bool IsAllowMultipleInstances => false;
        public string ViewName => _localSettings.ViewNames.DialogueWindow;

        public DialogueWindowFactory(
            IWindowViewProvider windowViewProvider,
            ILocalSettings localSettings)
        {
            _windowViewProvider = windowViewProvider;
            _localSettings = localSettings;
        }

        public async UniTask<IWindowPresenter> CreateAsync()
        {
            //TODO: Get MainMenuWindowModel from ModelProvider
            var model = new DialogueWindowModel(0);
            var dialogueWindowView = await _windowViewProvider.GetAsync<IDialogueWindowView>(ViewName, WindowType.Main);
            var dialogueWindowPresenter = new DialogueWindowPresenter();
            dialogueWindowPresenter.Init(dialogueWindowView, model);

            return dialogueWindowPresenter;
        }
    }
}