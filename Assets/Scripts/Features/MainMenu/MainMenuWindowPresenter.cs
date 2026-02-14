using Core.MVPImplementation;
using R3;
using UnityEngine;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowPresenter : BaseWindowPresenter<IMainMenuWindowView, MainMenuWindowModel>
    {
        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.NewGame.Subscribe(OnNewGame).AddTo(ref disposableBuilder);
            View.LoadGame.Subscribe(OnLoadGame).AddTo(ref disposableBuilder);
            View.ExitGame.Subscribe(OnExitGame).AddTo(ref disposableBuilder);
        }

        private void OnNewGame(Unit _)
        {
        }

        private void OnLoadGame(Unit _)
        {
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