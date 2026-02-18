using System.Collections.Generic;
using Ink.Runtime;

namespace Features.Narrative
{
    public class InkNarrativeEngine : INarrativeEngine
    {
        private Story _story;

        public bool CanContinue => _story.canContinue;

        public IReadOnlyList<NarrativeChoice> CurrentChoices
        {
            get
            {
                var list = new List<NarrativeChoice>();
                List<Choice> choices = _story.currentChoices;
                if (choices != null)
                {
                    for (int i = 0; i < choices.Count; i++)
                    {
                        Choice choice = choices[i];
                        list.Add(new NarrativeChoice(i, choice.text));
                    }
                }

                return list;
            }
        }

        public void LoadStory(string compiledInkAssetOrJson)
        {
            _story = new Story(compiledInkAssetOrJson);
        }

        public void ResetToStart()
        {
            _story.ResetState();
        }

        public void ChoosePath(string path)
        {
            _story.ChoosePathString(path);
        }

        public NarrativeLine Continue()
        {
            string text = _story.Continue();
            List<string> tags;
            if (_story.currentTags != null)
            {
                tags = new List<string>(_story.currentTags);
            }
            else
            {
                tags = new List<string>();
            }

            return new NarrativeLine(text, tags);
        }

        public void ChooseChoiceIndex(int index)
        {
            _story.ChooseChoiceIndex(index);
        }

        public string GetStateJson()
        {
            return _story.state.ToJson();
        }

        public void SetStateJson(string stateJson)
        {
            _story.state.LoadJson(stateJson);
        }
    }
}