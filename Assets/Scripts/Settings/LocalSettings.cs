using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(
        fileName = "LocalSettings",
        menuName = "Config/LocalSettings",
        order = 0
    )]
    public class LocalSettings : ScriptableObject, ILocalSettings
    {
        [SerializeField]
        private ViewNames _viewNames;

        [SerializeField]
        private ResourceNames _resourceNames;

        [SerializeField]
        public CharacterSettings _characterSettings;

        public IViewNames ViewNames => _viewNames;
        public ICharacterSettings CharacterSettings => _characterSettings;
        public IResourceNames ResourceNames => _resourceNames;
    }
}