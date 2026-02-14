using Core.MVPImplementation;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewInterfaces;

namespace Views.ChoiceWindow
{
    public class ChoiceListItemView : BaseView, IChoiceListItemView
    {
        [SerializeField]
        private TextMeshProUGUI _choiceNameText;

        [SerializeField]
        private Button _chooseButton;

        [SerializeField]
        private CanvasGroup _selectedBackground;

        private readonly ReactiveCommand _chose = new();

        public ReactiveProperty<string> ChoiceName { get; } = new();
        public Observable<Unit> Chose => _chose;

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            ChoiceName.Subscribe(UpdateChoiceName).AddTo(ref disposableBuilder);
            _chooseButton.OnClickAsObservable().Subscribe(OnChose).AddTo(ref disposableBuilder);
        }

        public void SetSelected(bool isSelected)
        {
            _selectedBackground.alpha = isSelected ? 1.0f : 0.0f;
        }

        private void UpdateChoiceName(string choiceName)
        {
            _choiceNameText.text = choiceName;
        }

        private void OnChose(Unit _)
        {
            _chose.Execute(Unit.Default);
        }
    }
}