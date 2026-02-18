namespace Features.Narrative
{
    public class CharacterData
    {
        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public string InkEntryPath { get; private set; }

        public CharacterData(string id, string displayName, string inkEntryPath)
        {
            Id = id;
            DisplayName = displayName;
            InkEntryPath = inkEntryPath;
        }
    }
}