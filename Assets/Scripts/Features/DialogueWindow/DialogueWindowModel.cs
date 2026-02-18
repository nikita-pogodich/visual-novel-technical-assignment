using Core.MVPImplementation;
using Features.Narrative;
using R3;

namespace Features.DialogueWindow
{
    public class DialogueWindowModel : BaseModel
    {
        public readonly ReactiveProperty<string> CurrentCharacterName = new();
        public readonly ReactiveProperty<string> CurrentDialogue = new();

        private readonly NarrativeModel _narrativeModel;

        public DialogueSnapshot CurrentDialogueSnapshot { get; private set; }

        public bool HasConversationChoices =>
            CurrentDialogueSnapshot.Choices.Count > 0 &&
            CurrentDialogueSnapshot.Mode == WorldMode.InConversation;

        public DialogueWindowModel(NarrativeModel narrativeModel, int uniqueId) : base(uniqueId)
        {
            _narrativeModel = narrativeModel;
        }

        public void Continue()
        {
            _narrativeModel.Continue();
            UpdateDialogue();
        }

        public void UpdateDialogue()
        {
            DialogueSnapshot dialogueSnapshot = _narrativeModel.GetSnapshot();
            CurrentDialogueSnapshot = dialogueSnapshot;

            if (dialogueSnapshot.TalkTargetsById.TryGetValue(
                    dialogueSnapshot.SpeakerKey,
                    out CharacterData characterData) == false)
            {
                return;
            }

            CurrentCharacterName.Value = characterData.DisplayName;
            CurrentDialogue.Value = dialogueSnapshot.LineText;
        }
    }
}