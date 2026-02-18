using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface ICharacterSelectionView : IView
    {
        Observable<ICharacterView> Selected { get; }
        void AddCharacter(ICharacterView characterView);
    }
}