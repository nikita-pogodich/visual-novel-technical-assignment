using System;

namespace Features.Narrative
{
    [Serializable]
    public class SaveData
    {
        public int Version;
        public string Mode;
        public string ActiveCharacterId;
        public string SpeakerKey;
        public string BackgroundKey;
        public string EndingKey;
        public string LineText;
        public string NarrativeStateJson;

        public SaveData(
            int version,
            string mode,
            string activeCharacterId,
            string speakerKey,
            string backgroundKey,
            string endingKey,
            string lineText,
            string narrativeStateJson)
        {
            Version = version;
            Mode = mode;
            ActiveCharacterId = activeCharacterId;
            SpeakerKey = speakerKey;
            BackgroundKey = backgroundKey;
            EndingKey = endingKey;
            LineText = lineText;
            NarrativeStateJson = narrativeStateJson;
        }
    }
}