using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IEndingWindowView : IWindowView
    {
        ReactiveProperty<string> EndingText { get; }
        Observable<Unit> ReturnToMainMenu { get; }
    }
}