using Core.MVP;

namespace ViewInterfaces
{
    public interface IChoiceWindowView : IWindowView
    {
        IChoicesListView ChoicesListView { get; }
    }
}