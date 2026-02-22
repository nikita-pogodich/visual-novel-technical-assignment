using System.Collections.Generic;

namespace Features.Narrative
{
    public class DialogueSnapshot
    {
        public WorldMode Mode { get; private set; }
        public string ActiveCharacterId { get; private set; }

        public string SpeakerKey { get; private set; }
        public string BackgroundKey { get; private set; }
        public string EndingKey { get; private set; }

        public string LineText { get; private set; }

        public IReadOnlyList<ChoiceData> Choices { get; private set; }
        public IReadOnlyDictionary<string, CharacterData> TalkTargetsById { get; private set; }

        public DialogueSnapshot(
            WorldMode mode,
            string activeCharacterId,
            string speakerKey,
            string backgroundKey,
            string endingKey,
            string lineText,
            IReadOnlyList<ChoiceData> choices,
            IReadOnlyDictionary<string, CharacterData> talkTargetsById)
        {
            Mode = mode;
            ActiveCharacterId = activeCharacterId;

            SpeakerKey = speakerKey;
            BackgroundKey = backgroundKey;
            EndingKey = endingKey;

            LineText = lineText;
            Choices = choices;
            TalkTargetsById = talkTargetsById;
        }
    }
}