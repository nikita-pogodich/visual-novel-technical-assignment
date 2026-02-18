using Core.MVPImplementation;
using Features.Narrative;
using R3;
using UnityEngine;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowPresenter : BaseWindowPresenter<IMainMenuWindowView, MainMenuWindowModel>
    {
        private readonly NarrativeController _narrativeController;

        public MainMenuWindowPresenter(NarrativeController narrativeController)
        {
            _narrativeController = narrativeController;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.NewGame.Subscribe(OnNewGame).AddTo(ref disposableBuilder);
            View.LoadGame.Subscribe(OnLoadGame).AddTo(ref disposableBuilder);
            View.ExitGame.Subscribe(OnExitGame).AddTo(ref disposableBuilder);
        }

        private void OnNewGame(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            SetShown(false);
            _narrativeController.StartNewGame();
        }

        private void OnLoadGame(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            SetShown(false);
            _narrativeController.LoadGame();
        }

        private void OnExitGame(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Application.Quit();
        }
    }
}