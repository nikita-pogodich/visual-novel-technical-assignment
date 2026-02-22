using Core.MVPImplementation;
using Features.Narrative;
using R3;

namespace Features.DialogueWindow
{
    public class DialogueWindowModel : BaseModel
    {
        public readonly ReactiveProperty<string> CurrentCharacterName = new();
        public readonly ReactiveProperty<string> CurrentDialogue = new();
        public NarrativeModel NarrativeModel => _narrativeModel;

        private readonly NarrativeModel _narrativeModel;

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
            if (_narrativeModel.CurrentTalkTargetsById.TryGetValue(
                    _narrativeModel.CurrentSpeakerKey,
                    out CharacterData characterData) == false)
            {
                return;
            }

            CurrentCharacterName.Value = characterData.DisplayName;
            CurrentDialogue.Value = _narrativeModel.CurrentLineText;
        }
    }
}