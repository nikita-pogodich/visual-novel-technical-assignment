using Core.MVPImplementation;
using R3;
using ViewInterfaces;

namespace Features.DialogueWindow
{
    public class DialogueWindowPresenter : BaseWindowPresenter<IDialogueWindowView, DialogueWindowModel>
    {
        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.Continue.Subscribe(OnContinue).AddTo(ref disposableBuilder);
        }

        private void OnContinue(Unit _)
        {
            //TODO: call continue in story model, get new dialogue and call SetDialogue and SetCharacterName
        }
    }
}