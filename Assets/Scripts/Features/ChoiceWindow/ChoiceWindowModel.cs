using System.Collections.Generic;
using Core.ModelProvider;
using Core.MVPImplementation;
using Features.ChoiceWindow.ChoicesList;
using Features.Narrative;

namespace Features.ChoiceWindow
{
    public class ChoiceWindowModel : BaseModel
    {
        public ChoicesListModel ChoicesListModel { get; }

        public ChoiceWindowModel(IModelProvider modelProvider, NarrativeModel narrativeModel, int uniqueId) : base(uniqueId)
        {
            ChoicesListModel = new ChoicesListModel(modelProvider, narrativeModel, modelProvider.GetUniqueId());
        }

        public void UpdateChoices(IReadOnlyList<ChoiceData> choices)
        {
            ChoicesListModel.UpdateChoices(choices);
        }
    }
}