using Core.MVPImplementation;
using Core.WindowManager;
using Features.ChoiceWindow;
using Features.Narrative;
using R3;
using Settings;
using ViewInterfaces;

namespace Features.DialogueWindow
{
    public class DialogueWindowPresenter : BaseWindowPresenter<IDialogueWindowView, DialogueWindowModel>
    {
        private readonly IWindowManager _windowManager;
        private readonly ILocalSettings _localSettings;
        private readonly NarrativeController _narrativeController;
        private readonly ChoiceWindowModel _choiceWindowModel;

        public DialogueWindowPresenter(
            IWindowManager windowManager,
            ILocalSettings localSettings,
            NarrativeController narrativeController,
            ChoiceWindowModel choiceWindowModel)
        {
            _windowManager = windowManager;
            _localSettings = localSettings;
            _narrativeController = narrativeController;
            _choiceWindowModel = choiceWindowModel;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.Continue.Subscribe(OnContinue).AddTo(ref disposableBuilder);
            Model.CurrentCharacterName.Subscribe(OnCurrentCharacterNameChanged).AddTo(ref disposableBuilder);
            Model.CurrentDialogue.Subscribe(OnCurrentDialogueChanged).AddTo(ref disposableBuilder);
            _choiceWindowModel.ChoicesListModel.Chose.Subscribe(OnChose).AddTo(ref disposableBuilder);
        }

        protected override void OnShow()
        {
            Model.UpdateDialogue();
            OnDialogueWindowUpdated();
        }

        private void OnContinue(Unit _)
        {
            Model.Continue();
            OnDialogueWindowUpdated();
        }

        private void OnCurrentCharacterNameChanged(string characterName)
        {
            View.SetCharacterName(characterName);
        }

        private void OnCurrentDialogueChanged(string dialogue)
        {
            View.SetDialogue(dialogue);
        }

        private void OnDialogueWindowUpdated()
        {
            if (Model.CurrentDialogueSnapshot.Mode == WorldMode.CharacterSelect)
            {
                SetShown(false);
                _narrativeController.EnableCharacterSelection(Model.CurrentDialogueSnapshot);
            }
            else if (Model.HasConversationChoices)
            {
                _windowManager.ShowWindowAsync<IChoiceWindowView, ChoiceWindowModel>(
                    _localSettings.ViewNames.ChoiceWindow,
                    beforeShow: OnBeforeChoiceWindowShow);
            }
        }

        private void OnBeforeChoiceWindowShow(ChoiceWindowModel choiceWindowModel)
        {
            choiceWindowModel.UpdateChoices(Model.CurrentDialogueSnapshot.Choices);
        }

        private void OnChose(Unit _)
        {
            Model.UpdateDialogue();
            OnDialogueWindowUpdated();
        }
    }
}