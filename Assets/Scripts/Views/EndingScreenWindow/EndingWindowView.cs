using Core.MVPImplementation;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewInterfaces;

namespace Views.EndingScreenWindow
{
    public class EndingWindowView : BaseWindowView, IEndingWindowView
    {
        [SerializeField]
        private TextMeshProUGUI _endingText;

        [SerializeField]
        private Button _returnToMainMenuButton;

        private readonly ReactiveCommand _returnToMainMenu = new();
        public ReactiveProperty<string> EndingText { get; } = new();
        public Observable<Unit> ReturnToMainMenu => _returnToMainMenu;

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            SetCanvasEnabled(isShown);
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            _returnToMainMenuButton.OnClickAsObservable().Subscribe(OnReturnToMainMenu).AddTo(ref disposableBuilder);
            EndingText.Subscribe(OnEndingTextChanged).AddTo(ref disposableBuilder);
        }

        private void OnReturnToMainMenu(Unit _)
        {
            _returnToMainMenu.Execute(Unit.Default);
        }

        private void OnEndingTextChanged(string endingText)
        {
            _endingText.text = endingText;
        }
    }
}