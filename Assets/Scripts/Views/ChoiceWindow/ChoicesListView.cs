using Core.MVPImplementation;
using UnityEngine;
using ViewInterfaces;

namespace Views.ChoiceWindow
{
    public class ChoicesListView : BaseView, IChoicesListView
    {
        [SerializeField]
        private RectTransform _content;

        public void AddItem(IChoiceListItemView item)
        {
            if (item is not BaseView itemView)
            {
                return;
            }

            Transform itemTransform = itemView.transform;
            itemTransform.SetParent(_content);
            itemTransform.localScale = Vector3.one;
            itemTransform.localEulerAngles = Vector3.zero;
        }
    }
}