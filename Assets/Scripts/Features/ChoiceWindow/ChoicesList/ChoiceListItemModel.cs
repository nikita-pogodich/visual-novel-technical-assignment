using Core.MVPImplementation;
using R3;

namespace Features.ChoiceWindow.ChoicesList
{
    public class ChoiceListItemModel : BaseModel
    {
        private readonly ReactiveProperty<bool> _isSelected = new();
        private readonly ReactiveCommand<int> _chose = new();
        
        public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        public Observable<int> Chose => _chose;
        public int ChoiceIndex { get; }
        public string ChoiceName { get; }

        public ChoiceListItemModel(int uniqueId, int choiceIndex, string choiceName) : base(uniqueId)
        {
            ChoiceIndex = choiceIndex;
            ChoiceName = choiceName;
        }

        public void ChoseChoice()
        {
            _chose.Execute(ChoiceIndex);
        }

        public void SetSelected(bool isSelected)
        {
            _isSelected.Value = isSelected;
        }
    }
}