using Core.MVP;
using Core.MVPImplementation;
using Core.ResourcesManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.ViewProvider
{
    public class ViewProvider : IViewProvider
    {
        private readonly IResourcesManager _resourcesManager;

        public ViewProvider(IResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        public TView Get<TView>(string resourceKey) where TView : IView
        {
            GameObject resource = _resourcesManager.Instantiate(resourceKey);
            TView view = resource.GetComponent<TView>();

            view.Init(resourceKey);

            return view;
        }

        public async UniTask<TView> GetAsync<TView>(string resourceKey) where TView : IView
        {
            GameObject resource = await _resourcesManager.InstantiateAsync(resourceKey);
            TView view = resource.GetComponent<TView>();

            view.Init(resourceKey);

            return view;
        }

        public void Release<TView>(string resourceKey, TView view) where TView : IView
        {
            if (view == null)
            {
                // _dualLogger.Mandatory.LogError("Trying to release null view");
                return;
            }

            if (view is not BaseView baseView)
            {
                // _dualLogger.Mandatory.LogError(
                    // $"Trying to release {view.GetType()}. {nameof(ViewProvider)} can release only {nameof(BaseView)}");
                return;
            }

            if (baseView == null)
            {
                return;
            }

            _resourcesManager.ReleaseGameObject(resourceKey, baseView.gameObject);
        }
    }
}