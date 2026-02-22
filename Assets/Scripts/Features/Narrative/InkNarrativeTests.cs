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
            Print(narrativeModel.GetSnapshot());

            narrativeModel.StartConversation("char1");
            Print(narrativeModel.GetSnapshot());

            narrativeModel.Continue();
            Print(narrativeModel.GetSnapshot());

            narrativeModel.Continue();
            Print(narrativeModel.GetSnapshot());

            ChooseByOrder(narrativeModel, 2);
            Print(narrativeModel.GetSnapshot());

            narrativeModel.StartConversation("char2");
            Print(narrativeModel.GetSnapshot());

            narrativeModel.Continue();
            Print(narrativeModel.GetSnapshot());

            narrativeModel.Continue();
            Print(narrativeModel.GetSnapshot());

            ChooseByOrder(narrativeModel, 1);
            Print(narrativeModel.GetSnapshot());
        }

        private void ChooseByOrder(NarrativeModel game, int order)
        {
            DialogueSnapshot snapshot = game.GetSnapshot();
            if (snapshot.Choices == null || snapshot.Choices.Count == 0)
            {
                throw new InvalidOperationException("No choices available to pick.");
            }

            int index = order - 1;
            if (index < 0 || index >= snapshot.Choices.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(order), "Choice order out of range.");
            }

            game.Choose(snapshot.Choices[index].Index);
        }

        private void Print(DialogueSnapshot snapshot)
        {
            Debug.Log("<color=green>=== SNAPSHOT ===</color>");
            Debug.Log("Mode: " + snapshot.Mode);
            Debug.Log("ActiveCharacter: " + (string.IsNullOrEmpty(snapshot.ActiveCharacterId) ? "none" : snapshot.ActiveCharacterId));
            Debug.Log("SpeakerKey: " + (snapshot.SpeakerKey ?? "null"));
            Debug.Log("BackgroundKey: " + (snapshot.BackgroundKey ?? "null"));
            Debug.Log("EndingKey: " + (snapshot.EndingKey ?? "null"));
            Debug.Log("Line: " + (snapshot.LineText ?? "null"));

            if (snapshot.Choices != null && snapshot.Choices.Count > 0)
            {
                Debug.Log("Choices:");
                for (int i = 0; i < snapshot.Choices.Count; i++)
                {
                    Debug.Log("  [" + snapshot.Choices[i].Index + "] " + snapshot.Choices[i].Text);
                }
            }
            else
            {
                Debug.Log("Choices: none");
            }

            if (snapshot.TalkTargetsById != null && snapshot.TalkTargetsById.Count > 0)
            {
                Debug.Log("TalkTargets:");
                foreach (CharacterData snapshotTalkTarget in snapshot.TalkTargetsById.Values)
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