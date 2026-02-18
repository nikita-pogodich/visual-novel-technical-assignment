using Core.SaveStore;
using Features.Narrative;
using Newtonsoft.Json;

namespace Features.SaveLoad
{
    public class SaveLoadManager
    {
        private readonly NarrativeModel _narrativeModel;
        private readonly INarrativeEngine _narrative;
        private readonly ISaveStore _store;

        public SaveLoadManager(NarrativeModel narrativeModel, INarrativeEngine narrative, ISaveStore store)
        {
            _narrativeModel = narrativeModel;
            _narrative = narrative;
            _store = store;
        }

        public void Save(string slotId)
        {
            _narrativeModel.CaptureNarrativeState(_narrative.GetStateJson());

            SaveData saveData = SaveLoadUseCases.ToSaveData(_narrativeModel);
            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            _store.Save(slotId, json);
        }

        public bool Load(string slotId)
        {
            string json = _store.Load(slotId);

            SaveData saveData;
            try
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch
            {
                return false;
            }

            if (saveData == null)
            {
                return false;
            }

            SaveLoadUseCases.ApplySaveData(_narrativeModel, saveData);

            if (string.IsNullOrWhiteSpace(saveData.NarrativeStateJson) == false)
            {
                _narrative.SetStateJson(saveData.NarrativeStateJson);
            }

            return true;
        }
    }
}