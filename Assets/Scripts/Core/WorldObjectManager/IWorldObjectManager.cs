using Core.MVP;
using Cysharp.Threading.Tasks;

namespace Core.WorldObjectManager
{
    public interface IWorldObjectManager
    {
        UniTask CreateWorldObjectAsync<TPresenter, TView, TModel>(string viewName, TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel;

        void ReleaseWorldObject<TView, TModel>(TModel model)
            where TView : IWorldObjectView
            where TModel : IModel;
    }
}