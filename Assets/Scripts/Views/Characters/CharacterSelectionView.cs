using Core.MVPImplementation;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using ViewInterfaces;

namespace Views.Characters
{
    public class CharacterSelectionView : BaseView, ICharacterSelectionView
    {
        [SerializeField]
        private LayerMask _selectableLayers = ~0;

        [SerializeField]
        private float _maxDistance = 1000f;

        private Camera _camera;
        private InputAction _submitInputAction;
        private InputAction _pointInputAction;
        private readonly RaycastHit[] _raycastHits = new RaycastHit[16];
        private readonly ReactiveCommand<ICharacterView> _selected = new();

        public Observable<ICharacterView> Selected => _selected;

        public void AddCharacter(ICharacterView characterView)
        {
            if (characterView is not BaseView itemView)
            {
                return;
            }

            Transform itemTransform = itemView.transform;
            itemTransform.SetParent(transform);
            itemTransform.localScale = Vector3.one;
            itemTransform.localEulerAngles = Vector3.zero;
        }

        private void Start()
        {
            _camera = Camera.main;
            _submitInputAction = InputSystem.actions.FindAction("Click");
            _pointInputAction = InputSystem.actions.FindAction("Point");
        }

        private void Update()
        {
            ProcessCharacterSelection();
        }

        private void ProcessCharacterSelection()
        {
            if (_submitInputAction.WasPressedThisFrame() == false)
            {
                return;
            }

            Vector2 screenPos = _pointInputAction.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(screenPos);

            int hitCount = Physics.RaycastNonAlloc(
                ray,
                _raycastHits,
                _maxDistance,
                _selectableLayers,
                QueryTriggerInteraction.Ignore
            );

            if (hitCount <= 0)
            {
                return;
            }

            int bestIndex = 0;
            float bestDist = _raycastHits[0].distance;

            for (int i = 1; i < hitCount; i++)
            {
                float distance = _raycastHits[i].distance;
                if (distance < bestDist)
                {
                    bestDist = distance;
                    bestIndex = i;
                }
            }

            GameObject selected = _raycastHits[bestIndex].collider.gameObject;
            var characterView = selected.GetComponent<ICharacterView>();
            if (characterView != null)
            {
                _selected.Execute(characterView);
            }
        }
    }
}