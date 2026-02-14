using Core.MVPImplementation;
using R3;
using ViewInterfaces;

namespace Features.ChoiceWindow.ChoicesList
{
    public class ChoiceListItemPresenter : BasePresenter<IChoiceListItemView, ChoiceListItemModel>
    {
        private bool _isSelected;

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            View.ChoiceName.Value = Model.ChoiceName;

            View.Chose.Subscribe(OnChose).AddTo(ref disposableBuilder);
            Model.IsSelected.Subscribe(View.SetSelected).AddTo(ref disposableBuilder);
        }

        private void OnChose(Unit _)
        {
            if (IsShown == false)
            {
                return;
            }

            Model.ChoseChoice();
        }
    }
}