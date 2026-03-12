using Core.MVPImplementation;
using Core.SaveSystem;
using Features.Narrative;
using R3;
using Settings;
using UnityEngine;
using ViewInterfaces;

namespace Features.MainMenu
{
    public class MainMenuWindowPresenter : BaseWindowPresenter<IMainMenuWindowView, MainMenuWindowModel>
    {
        private readonly ILocalSettings _localSettings;
        private readonly ISaveSystem _saveSystem;
        private readonly NarrativeController _narrativeController;

        public MainMenuWindowPresenter(
            ILocalSettings localSettings,
            ISaveSystem saveSystem,
            NarrativeController narrativeController)
        {
            _localSettings = localSettings;
            _saveSystem = saveSystem;
            _narrativeController = narrativeController;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.NewGame.Subscribe(OnNewGame).AddTo(ref disposableBuilder);
            View.LoadGame.Subscribe(OnLoadGame).AddTo(ref disposableBuilder);
            View.ExitGame.Subscribe(OnExitGame).AddTo(ref disposableBuilder);
        }

        protected override void OnShow()
        {
            bool hasAnySaveFiles = _saveSystem.SlotExists(_localSettings.GameSettings.AutoSaveSlotName);
            View.SetLoadButtonShown(hasAnySaveFiles);
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