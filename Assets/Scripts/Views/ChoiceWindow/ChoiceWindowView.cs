using Core.MVPImplementation;
using UnityEngine;
using ViewInterfaces;

namespace Views.ChoiceWindow
{
    public class ChoiceWindowView : BaseWindowView, IChoiceWindowView
    {
        [SerializeField]
        private ChoicesListView _choicesListView;

        public IChoicesListView ChoicesListView => _choicesListView;

        public override void SetShown(bool isShown)
        {
            base.SetShown(isShown);
            SetCanvasEnabled(isShown);
        }
    }
}