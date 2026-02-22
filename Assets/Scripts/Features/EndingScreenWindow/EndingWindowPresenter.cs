using Core.MVPImplementation;
using Core.WindowManager;
using Features.MainMenu;
using R3;
using Settings;
using ViewInterfaces;

namespace Features.EndingScreenWindow
{
    public class EndingWindowPresenter : BaseWindowPresenter<IEndingWindowView, EndingWindowModel>
    {
        private readonly IWindowManager _windowManager;
        private readonly ILocalSettings _localSettings;

        public EndingWindowPresenter(IWindowManager windowManager, ILocalSettings localSettings)
        {
            _windowManager = windowManager;
            _localSettings = localSettings;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.ReturnToMainMenu.Subscribe(OnReturnToMainMenu).AddTo(ref disposableBuilder);
        }

        protected override void OnShow()
        {
            View.EndingText.Value = Model.EndingText;
        }

        private void OnReturnToMainMenu(Unit _)
        {
            SetShown(false);
            _windowManager.ShowWindowAsync<IMainMenuWindowView, MainMenuWindowModel>(
                _localSettings.ViewNames.MainMenuWindow);
        }
    }
}