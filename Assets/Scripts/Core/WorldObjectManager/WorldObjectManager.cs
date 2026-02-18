using System;
using System.Collections.Generic;
using Core.MVP;
using Cysharp.Threading.Tasks;

namespace Core.WorldObjectManager
{
    public class WorldObjectManager : IWorldObjectManager, IDisposable
    {
        private readonly Dictionary<string, IWorldObjectFactory> _worldObjectFactoriesByViewName = new();
        private readonly Dictionary<IModel, IPresenter> _presentersByModel = new();

        public void RegisterFactory(IWorldObjectFactory worldObjectFactory)
        {
            _worldObjectFactoriesByViewName.TryAdd(worldObjectFactory.ViewName, worldObjectFactory);
        }

        public void Dispose()
        {
            foreach (IPresenter presenter in _presentersByModel.Values)
            {
                presenter.Deinit();
            }

            _worldObjectFactoriesByViewName.Clear();
            _presentersByModel.Clear();
        }

        public async UniTask CreateWorldObjectAsync<TPresenter, TView, TModel>(string viewName, TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel
        {
            if (_worldObjectFactoriesByViewName.TryGetValue(
                    viewName,
                    out IWorldObjectFactory worldObjectFactory) == false)
            {
                await UniTask.CompletedTask;
                return;
            }

            IWorldObjectPresenter presenter = await worldObjectFactory.CreateAsync<TPresenter, TView, TModel>(model);
            if (presenter is not IWorldObjectPresenter<TView, TModel> worldObjectPresenter)
            {
                await UniTask.CompletedTask;
                return;
            }

            _presentersByModel.Add(worldObjectPresenter.Model, worldObjectPresenter);
            worldObjectPresenter.SetShown(true);
        }

        public void ReleaseWorldObject<TView, TModel>(TModel model)
            where TView : IWorldObjectView
            where TModel : IModel
        {
            if (_presentersByModel.TryGetValue(model, out IPresenter presenter) == false)
            {
                return;
            }

            if (presenter is not IWorldObjectPresenter<TView, TModel> worldObjectPresenter)
            {
                return;
            }

            string viewName = worldObjectPresenter.View.ViewName;
            if (_worldObjectFactoriesByViewName.TryGetValue(
                    viewName,
                    out IWorldObjectFactory worldObjectFactory) == false)
            {
                return;
            }

            worldObjectPresenter.SetShown(false);
            worldObjectFactory.Release(worldObjectPresenter);
        }
    }
}