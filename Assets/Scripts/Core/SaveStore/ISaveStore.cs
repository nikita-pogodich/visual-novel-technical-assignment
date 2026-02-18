namespace Core.SaveStore
{
    public interface ISaveStore
    {
        void Save(string slotId, string json);
        string Load(string slotId);
        bool Exists(string slotId);
    }
}
