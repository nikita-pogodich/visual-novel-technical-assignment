namespace Features.Narrative
{
    public class SaveData
    {
        public int Version { get; set; }
        public string Mode { get; set; }
        public string ActiveCharacterId { get; set; }
        public string SpeakerKey { get; set; }
        public string BackgroundKey { get; set; }
        public string EndingKey { get; set; }
        public string LineText { get; set; }
        public string NarrativeStateJson { get; set; }

        public SaveData()
        {
            Version = 1;
            Mode = "CharacterSelect";
        }
    }
}
