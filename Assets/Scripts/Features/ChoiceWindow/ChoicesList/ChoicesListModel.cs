using System.Collections.Generic;
using Core.MVPImplementation;
using Cysharp.Threading.Tasks;
using R3;

namespace Features.ChoiceWindow.ChoicesList
{
    public class ChoicesListModel : BaseModel
    {
        private readonly Dictionary<int, ChoiceListItemModel> _choiceModelsByIndex = new();
        private readonly ReactiveProperty<ChoiceListItemModel> _selectedRule = new();
        private readonly CompositeDisposable _reactiveCompositeDisposable = new();

        public IReadOnlyDictionary<int, ChoiceListItemModel> ChoiceModelsByIndex => _choiceModelsByIndex;
        public ReadOnlyReactiveProperty<ChoiceListItemModel> SelectedChoice => _selectedRule;

        public ChoicesListModel(int uniqueId) : base(uniqueId)
        {
        }

        public void ChooseChoice(int index)
        {
            //TODO: Call choose choice in narrative manager
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

        public void SelectChoice(int index)
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

        protected override async UniTask OnInit()
        {
            List<int> choicesIndices = new List<int> {1, 2, 3, 4, 5};

            foreach (int choiceIndex in choicesIndices)
            {
                //TODO: get model from ModelProvider
                var choiceListItemModel = new ChoiceListItemModel(choiceIndex, choiceIndex, choiceIndex.ToString());
                choiceListItemModel.Chose.Subscribe(ChooseChoice).AddTo(_reactiveCompositeDisposable);
                _choiceModelsByIndex.Add(choiceIndex, choiceListItemModel);
            }

            SelectChoice(0);

            await UniTask.CompletedTask;
        }

        protected override void OnDeinit()
        {
            _choiceModelsByIndex.Clear();
            _reactiveCompositeDisposable.Dispose();
        }
    }
}