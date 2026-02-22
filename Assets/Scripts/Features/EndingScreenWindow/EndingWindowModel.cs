using Core.MVPImplementation;

namespace Features.EndingScreenWindow
{
    public class EndingWindowModel : BaseModel
    {
        public string EndingKey { get; private set; }
        public string EndingText { get; private set; }

        public EndingWindowModel(int uniqueId) : base(uniqueId)
        {
        }

        public void UpdateModel(string endingKey, string endingText)
        {
            EndingKey = endingKey;
            EndingText = endingText;
        }
    }
}