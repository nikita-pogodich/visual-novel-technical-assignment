using System.Collections.Generic;
using Core.ModelProvider;
using Core.MVPImplementation;
using Features.Narrative;
using R3;

namespace Features.ChoiceWindow.ChoicesList
{
    public class ChoicesListModel : BaseModel
    {
        private readonly IModelProvider _modelProvider;
        private readonly NarrativeModel _narrativeModel;
        private readonly Dictionary<int, ChoiceListItemModel> _choiceModelsByIndex = new();
        private readonly ReactiveProperty<ChoiceListItemModel> _selectedRule = new();
        private readonly CompositeDisposable _reactiveCompositeDisposable = new();
        private readonly ReactiveCommand _chose = new();

        public IReadOnlyDictionary<int, ChoiceListItemModel> ChoiceModelsByIndex => _choiceModelsByIndex;
        public ReadOnlyReactiveProperty<ChoiceListItemModel> SelectedChoice => _selectedRule;

        public Observable<Unit> Chose => _chose;

        public ChoicesListModel(
            IModelProvider modelProvider,
            NarrativeModel narrativeModel,
            int uniqueId) : base(uniqueId)
        {
            _modelProvider = modelProvider;
            _narrativeModel = narrativeModel;
        }

        public void ChooseChoice(int index)
        {
            _narrativeModel.Choose(index);
            _chose.Execute(Unit.Default);
        }

        public void SelectUp()
        {
            int resultChoiceIndex;
            int selectedChoiceIndex = _selectedRule.CurrentValue.ChoiceIndex;
            if (selectedChoiceIndex == 0)
            {
                resultChoiceIndex = _choiceModelsByIndex.Count - 1;
            }
            else
            {
                resultChoiceIndex = selectedChoiceIndex - 1;
            }

            SelectChoice(resultChoiceIndex);
        }

        public void SelectDown()
        {
            int resultChoiceIndex;
            int selectedChoiceIndex = _selectedRule.CurrentValue.ChoiceIndex;
            if (selectedChoiceIndex == _choiceModelsByIndex.Count - 1)
            {
                resultChoiceIndex = 0;
            }
            else
            {
                resultChoiceIndex = selectedChoiceIndex + 1;
            }

            SelectChoice(resultChoiceIndex);
        }

        public void UpdateChoices(IReadOnlyList<ChoiceData> choices)
        {
            _choiceModelsByIndex.Clear();

            foreach (ChoiceData choice in choices)
            {
                var choiceListItemModel = new ChoiceListItemModel(
                    _modelProvider.GetUniqueId(),
                    choice.Index,
                    choice.Text);

                choiceListItemModel.Chose.Subscribe(ChooseChoice).AddTo(_reactiveCompositeDisposable);
                _choiceModelsByIndex.Add(choice.Index, choiceListItemModel);
            }

            SelectChoice(0);
        }

        protected override void OnDeinit()
        {
            _choiceModelsByIndex.Clear();
            _reactiveCompositeDisposable.Dispose();
        }

        private void SelectChoice(int index)
        {
            if (_choiceModelsByIndex.TryGetValue(index, out ChoiceListItemModel choiceListItemModel) == false)
            {
                return;
            }

            foreach (ChoiceListItemModel choiceListItemModelToDeselect in _choiceModelsByIndex.Values)
            {
                choiceListItemModelToDeselect.SetSelected(false);
            }

            choiceListItemModel.SetSelected(true);
            _selectedRule.Value = choiceListItemModel;
        }
    }
}