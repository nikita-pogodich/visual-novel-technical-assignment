using Core.MVP;
using R3;

namespace ViewInterfaces
{
    public interface IDialogueWindowView : IWindowView
    {
        Observable<Unit> Continue { get; }
        void SetDialogue(string dialogue);
        void SetCharacterName(string characterName);
    }
}