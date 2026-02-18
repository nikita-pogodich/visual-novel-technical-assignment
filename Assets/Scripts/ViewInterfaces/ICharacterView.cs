using Core.MVP;
using Core.ResourcesManager;
using UnityEngine;

namespace ViewInterfaces
{
    public interface ICharacterView : IView
    {
        public string CharacterId { get; set; }
        void InjectDependencies(IResourcesManager resourcesManager);
        void UpdateView(string characterId, string characterVisualKey, Vector3 position, Quaternion rotation);
    }
}