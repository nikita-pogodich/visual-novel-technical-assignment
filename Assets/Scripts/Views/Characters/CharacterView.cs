using Core.MVPImplementation;
using Core.ResourcesManager;
using UnityEngine;
using ViewInterfaces;

namespace Views.Characters
{
    public class CharacterView : BaseView, ICharacterView
    {
        private IResourcesManager _resourcesManager;
        private GameObject _characterVisualGameObject;
        private string _characterVisualKey;

        public string CharacterId { get; set; }

        public void InjectDependencies(IResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        public void UpdateView(string characterId, string characterVisualKey, Vector3 position, Quaternion rotation)
        {
            _characterVisualKey = characterVisualKey;
            CharacterId = characterId;

            _characterVisualGameObject = _resourcesManager.Instantiate(characterVisualKey, transform);
            Transform characterVisualTransform = _characterVisualGameObject.transform;
            characterVisualTransform.localPosition = Vector3.zero;
            characterVisualTransform.localRotation = Quaternion.identity;
            characterVisualTransform.localScale = Vector3.one;
            transform.position = position;
            transform.rotation = rotation;
        }

        protected override void OnDeinit()
        {
            if (_characterVisualGameObject != null)
            {
                _resourcesManager.ReleaseGameObject(_characterVisualKey, _characterVisualGameObject);
            }
        }
    }
}