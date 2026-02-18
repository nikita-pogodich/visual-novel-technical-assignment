using Core.MVPImplementation;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewInterfaces;

namespace Views.DialogueWindow
{
    public class DialogueWindowView : BaseWindowView, IDialogueWindowView
    {
        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private TextMeshProUGUI _characterName;

        [SerializeField]
        private TextMeshProUGUI _dialogue;

        private readonly ReactiveCommand _continue = new();
        public Observable<Unit> Continue => _continue;

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            SetCanvasEnabled(isShown);
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _continueButton.OnClickAsObservable().Subscribe(OnContinue).AddTo(ref disposableBuilder);
        }

        public void SetDialogue(string dialogue)
        {
            _dialogue.text = dialogue;
        }

        public void SetCharacterName(string characterName)
        {
            _characterName.text = characterName;
        }

        private void OnContinue(Unit _)
        {
            _continue.Execute(Unit.Default);
        }
    }
}