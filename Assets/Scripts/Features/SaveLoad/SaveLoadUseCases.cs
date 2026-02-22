using System;
using Features.Narrative;

namespace Features.SaveLoad
{
    public static class SaveLoadUseCases
    {
        public static SaveData ToSaveData(NarrativeModel narrativeModel)
        {
            var saveData = new SaveData();
            saveData.Mode = narrativeModel.Mode.ToString();
            saveData.ActiveCharacterId = narrativeModel.ActiveCharacter;
            saveData.SpeakerKey = narrativeModel.CurrentSpeakerKey;
            saveData.BackgroundKey = narrativeModel.CurrentBackgroundKey;
            saveData.EndingKey = narrativeModel.CurrentEndingKey;
            saveData.LineText = narrativeModel.CurrentLineText;
            saveData.NarrativeStateJson = narrativeModel.NarrativeStateJson;

            return saveData;
        }

        public static void ApplySaveData(NarrativeModel narrativeModel, SaveData data)
        {
            if (Enum.TryParse(data.Mode, out WorldMode mode) == false)
            {
                mode = WorldMode.CharacterSelect;
            }

            if (mode == WorldMode.InConversation && string.IsNullOrWhiteSpace(data.ActiveCharacterId) == false)
            {
                narrativeModel.BeginConversation(data.ActiveCharacterId);
            }

            narrativeModel.ApplyTagEffects(data.SpeakerKey, data.BackgroundKey, data.EndingKey);
            narrativeModel.ApplyLine(data.LineText);
            narrativeModel.CaptureNarrativeState(data.NarrativeStateJson);

            narrativeModel.ApplyChoices(null);
        }
    }
}