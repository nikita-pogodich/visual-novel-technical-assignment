namespace Settings
{
    public interface IGameSettings
    {
        string SavesFolderName { get; }
        string AutoSaveSlotName { get; }
    }
}