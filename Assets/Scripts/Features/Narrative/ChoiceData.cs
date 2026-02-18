namespace Features.Narrative
{
    public class ChoiceData
    {
        public int Index { get; private set; }
        public string Text { get; private set; }

        public ChoiceData(int index, string text)
        {
            Index = index;
            Text = text;
        }
    }
}
