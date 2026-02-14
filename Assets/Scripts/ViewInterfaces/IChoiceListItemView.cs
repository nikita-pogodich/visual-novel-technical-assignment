using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IChoiceListItemView : IView
    {
        ReactiveProperty<string> ChoiceName { get; }
        Observable<Unit> Chose { get; }
        void SetSelected(bool isSelected);
    }
}