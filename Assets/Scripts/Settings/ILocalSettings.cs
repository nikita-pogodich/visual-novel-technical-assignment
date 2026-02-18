namespace Settings
{
    public interface ILocalSettings
    {
        IViewNames ViewNames { get; }
        ICharacterSettings CharacterSettings { get; }
        IResourceNames ResourceNames { get; }
    }
}