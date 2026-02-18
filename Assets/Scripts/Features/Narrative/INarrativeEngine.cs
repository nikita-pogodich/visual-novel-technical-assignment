using System.Collections.Generic;

namespace Features.Narrative
{
    public class NarrativeLine
    {
        public string Text { get; private set; }
        public IReadOnlyList<string> Tags { get; private set; }

        public NarrativeLine(string text, IReadOnlyList<string> tags)
        {
            Text = text;
            Tags = tags;
        }
    }

    public sealed class NarrativeChoice
    {
        public int Index { get; private set; }
        public string Text { get; private set; }

        public NarrativeChoice(int index, string text)
        {
            Index = index;
            Text = text;
        }
    }

    public interface INarrativeEngine
    {
        void LoadStory(string compiledInkAssetOrJson);
        void ResetToStart();
        void ChoosePath(string path);

        bool CanContinue { get; }
        NarrativeLine Continue();

        IReadOnlyList<NarrativeChoice> CurrentChoices { get; }
        void ChooseChoiceIndex(int index);

        string GetStateJson();
        void SetStateJson(string stateJson);
    }
}
