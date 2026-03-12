using System;
using Core.SaveSystem;
using Features.Narrative;

namespace Features.SaveLoad
{
    public class SaveLoadManager
    {
        private readonly NarrativeModel _narrativeModel;
        private readonly INarrativeEngine _narrative;
        private readonly ISaveSystem _saveSystem;

        public SaveLoadManager(NarrativeModel narrativeModel, INarrativeEngine narrative, ISaveSystem saveSystem)
        {
            _narrativeModel = narrativeModel;
            _narrative = narrative;
            _saveSystem = saveSystem;
        }

        public void Save(string slotId)
        {
            _narrativeModel.CaptureNarrativeState(_narrative.GetStateJson());

            var saveData = new SaveData(
                version: 1,
                _narrativeModel.CurrentMode.ToString(),
                _narrativeModel.ActiveCharacterId,
                _narrativeModel.CurrentSpeakerKey,
                _narrativeModel.CurrentBackgroundKey,
                _narrativeModel.CurrentEndingKey,
                _narrativeModel.CurrentLineText,
                _narrativeModel.NarrativeStateJson);

            _saveSystem.Save(slotId, saveData);
        }

        public bool Load(string slotId)
        {
            if (_saveSystem.TryLoad(slotId, out SaveData saveData) == false)
            {
                return false;
            }

            if (Enum.TryParse(saveData.Mode, out WorldMode mode) == false)
            {
                mode = WorldMode.CharacterSelect;
            }

            if (mode == WorldMode.InConversation && string.IsNullOrWhiteSpace(saveData.ActiveCharacterId) == false)
            {
                _narrativeModel.BeginConversation(saveData.ActiveCharacterId);
            }

            _narrativeModel.ApplyTagEffects(saveData.SpeakerKey, saveData.BackgroundKey, saveData.EndingKey);
            _narrativeModel.ApplyLine(saveData.LineText);
            _narrativeModel.CaptureNarrativeState(saveData.NarrativeStateJson);

            _narrativeModel.ApplyChoices(null);

            if (string.IsNullOrWhiteSpace(saveData.NarrativeStateJson) == false)
            {
                _narrative.SetStateJson(saveData.NarrativeStateJson);
            }

            return true;
        }
    }
}