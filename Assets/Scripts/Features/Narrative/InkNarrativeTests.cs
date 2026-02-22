using System;
using UnityEngine;

namespace Features.Narrative
{
    public class InkNarrativeTests : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _story;

        private void Start()
        {
            RunGoodEnding(_story);
        }

        private void RunGoodEnding(TextAsset compiledInkJson)
        {
            var c1 = new CharacterData(id: "char1", displayName: "Character 1", inkEntryPath: "char1_root");
            var c2 = new CharacterData(id: "char2", displayName: "Character 2", inkEntryPath: "char2_root");

            INarrativeEngine narrative = new InkNarrativeEngine();
            var narrativeModel = new NarrativeModel(new[] {c1, c2}, narrative);

            narrativeModel.StartNewStory(compiledInkJson.text);
            Print(narrativeModel);

            narrativeModel.StartConversation("char1");
            Print(narrativeModel);

            narrativeModel.Continue();
            Print(narrativeModel);

            narrativeModel.Continue();
            Print(narrativeModel);

            ChooseByOrder(narrativeModel, 2);
            Print(narrativeModel);

            narrativeModel.StartConversation("char2");
            Print(narrativeModel);

            narrativeModel.Continue();
            Print(narrativeModel);

            narrativeModel.Continue();
            Print(narrativeModel);

            ChooseByOrder(narrativeModel, 1);
            Print(narrativeModel);
        }

        private void ChooseByOrder(NarrativeModel narrativeModel, int order)
        {
            if (narrativeModel.CurrentChoices == null || narrativeModel.CurrentChoices.Count == 0)
            {
                throw new InvalidOperationException("No choices available to pick.");
            }

            int index = order - 1;
            if (index < 0 || index >= narrativeModel.CurrentChoices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(order), "Choice order out of range.");
            }

            narrativeModel.Choose(narrativeModel.CurrentChoices[index].Index);
        }

        private void Print(NarrativeModel narrativeModel)
        {
            Debug.Log("<color=green>=== SNAPSHOT ===</color>");
            Debug.Log("Mode: " + narrativeModel.CurrentMode);
            Debug.Log("ActiveCharacter: " + (string.IsNullOrEmpty(narrativeModel.ActiveCharacterId) ? "none" : narrativeModel.ActiveCharacterId));
            Debug.Log("SpeakerKey: " + (narrativeModel.CurrentSpeakerKey ?? "null"));
            Debug.Log("BackgroundKey: " + (narrativeModel.CurrentBackgroundKey ?? "null"));
            Debug.Log("EndingKey: " + (narrativeModel.CurrentEndingKey ?? "null"));
            Debug.Log("Line: " + (narrativeModel.CurrentLineText ?? "null"));

            if (narrativeModel.CurrentChoices != null && narrativeModel.CurrentChoices.Count > 0)
            {
                Debug.Log("Choices:");
                for (int i = 0; i < narrativeModel.CurrentChoices.Count; i++)
                {
                    Debug.Log("  [" + narrativeModel.CurrentChoices[i].Index + "] " + narrativeModel.CurrentChoices[i].Text);
                }
            }
            else
            {
                Debug.Log("Choices: none");
            }

            if (narrativeModel.CurrentTalkTargetsById != null && narrativeModel.CurrentTalkTargetsById.Count > 0)
            {
                Debug.Log("TalkTargets:");
                foreach (CharacterData snapshotTalkTarget in narrativeModel.CurrentTalkTargetsById.Values)
                {
                    Debug.Log("  [" + snapshotTalkTarget.Id + "] " + snapshotTalkTarget.DisplayName);
                }
            }
            else
            {
                Debug.Log("TalkTargets: none");
            }
        }
    }
}