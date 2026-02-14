using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IMainMenuWindowView : IWindowView
    {
        Observable<Unit> NewGame { get; }
        Observable<Unit> LoadGame { get; }
        Observable<Unit> ExitGame { get; }
    }
}