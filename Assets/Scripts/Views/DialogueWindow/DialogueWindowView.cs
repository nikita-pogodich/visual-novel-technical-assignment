using Core.MVPImplementation;
using R3;
using UnityEngine;
using UnityEngine.UI;
using ViewInterfaces;

namespace Views.DialogueWindow
{
    public class DialogueWindowView : BaseWindowView, IDialogueWindowView
    {
        [SerializeField]
        private Button _continueButton;

        private readonly ReactiveCommand _continue = new();
        public Observable<Unit> Continue => _continue;

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _continueButton.OnClickAsObservable().Subscribe().AddTo(ref disposableBuilder);
        }

        public void SetDialogue(string dialogue)
        {
        }

        public void SetCharacterName(string characterName)
        {
        }

        private void OnContinue(Unit _)
        {
            _continue.Execute(Unit.Default);
        }
    }
}