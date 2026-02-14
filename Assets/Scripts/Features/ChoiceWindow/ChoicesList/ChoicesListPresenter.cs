using System.Collections.Generic;
using Core.MVPImplementation;
using Core.ViewProvider;
using R3;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using ViewInterfaces;

namespace Features.ChoiceWindow.ChoicesList
{
    public class ChoicesListPresenter : BasePresenter<IChoicesListView, ChoicesListModel>
    {
        private readonly IViewProvider _viewProvider;
        private readonly ILocalSettings _localSettings;
        private readonly List<ChoiceListItemPresenter> _choiceListItemPresenters = new();
        private InputAction _submitInputAction;
        private InputAction _navigateInputAction;

        public ChoicesListPresenter(IViewProvider viewProvider, ILocalSettings localSettings)
        {
            _viewProvider = viewProvider;
            _localSettings = localSettings;
        }

        protected override void OnInit(ref DisposableBuilder disposableBuilder)
        {
            CreateChoicesList();
            _submitInputAction = InputSystem.actions.FindAction("Submit");
            _navigateInputAction = InputSystem.actions.FindAction("Navigate");
            _submitInputAction.performed += OnSubmit;
            _navigateInputAction.performed += OnNavigate;
        }

        protected override void OnDeinit()
        {
            ClearChoicesList();
            _submitInputAction.performed -= OnSubmit;
            _navigateInputAction.performed -= OnNavigate;
        }

        private void CreateChoicesList()
        {
            foreach (ChoiceListItemModel choiceListItemModel in Model.ChoiceModelsByIndex.Values)
            {
                IChoiceListItemView choiceListItemView =
                    _viewProvider.Get<IChoiceListItemView>(_localSettings.ViewNames.ChoiceListItemView);

                var choicesListItemPresenter = new ChoiceListItemPresenter();
                choicesListItemPresenter.Init(choiceListItemView, choiceListItemModel);

                _choiceListItemPresenters.Add(choicesListItemPresenter);
                AddChildPresenter(choicesListItemPresenter);

                View.AddItem(choiceListItemView);
            }
        }

        private void ClearChoicesList()
        {
            for (int i = 0; i < _choiceListItemPresenters.Count; i++)
            {
                ChoiceListItemPresenter choiceListItemPresenter = _choiceListItemPresenters[i];
                RemoveChildPresenter(choiceListItemPresenter);

                IChoiceListItemView choiceListItemView = choiceListItemPresenter.View;
                choiceListItemView.Deinit();

                _viewProvider.Release(_localSettings.ViewNames.ChoiceListItemView, choiceListItemView);
            }

            _choiceListItemPresenters.Clear();
        }

        private void OnSubmit(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.phase == InputActionPhase.Performed)
            {
                Model.ChooseChoice(Model.SelectedChoice.CurrentValue.ChoiceIndex);
            }
        }

        private void OnNavigate(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.phase == InputActionPhase.Performed)
            {
                Vector2 navigateValue = callbackContext.ReadValue<Vector2>();
                if (navigateValue.y < 0)
                {
                    Model.SelectDown();
                }
                else if (navigateValue.y > 0)
                {
                    Model.SelectUp();
                }
            }
        }
    }
}