using System.IO;

namespace Core.SaveStore
{
    public class JsonFileSaveStore : ISaveStore
    {
        private readonly string _rootDir;

        public JsonFileSaveStore(string rootDir)
        {
            _rootDir = rootDir;
            Directory.CreateDirectory(_rootDir);
        }

        public void Save(string slotId, string json)
        {
            File.WriteAllText(GetPath(slotId), json);
        }

        public string Load(string slotId)
        {
            string path = GetPath(slotId);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return null;
        }

        public bool Exists(string slotId)
        {
            return File.Exists(GetPath(slotId));
        }

        private string GetPath(string slotId)
        {
            foreach (char fileNameChar in Path.GetInvalidFileNameChars())
            {
                slotId = slotId.Replace(fileNameChar.ToString(), "_");
            }

            return Path.Combine(_rootDir, slotId + ".json");
        }
    }
}