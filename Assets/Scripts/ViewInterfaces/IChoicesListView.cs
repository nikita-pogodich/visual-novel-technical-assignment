using Core.MVP;

namespace ViewInterfaces
{
    public interface IChoicesListView : IView
    {
        void AddItem(IChoiceListItemView item);
    }
}