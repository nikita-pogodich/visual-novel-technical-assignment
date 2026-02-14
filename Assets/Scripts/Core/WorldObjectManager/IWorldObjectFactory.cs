using Core.MVP;
using Cysharp.Threading.Tasks;

namespace Core.WorldObjectManager
{
    public interface IWorldObjectFactory
    {
        string ViewName { get; }

        UniTask<IWorldObjectPresenter> CreateAsync<TPresenter, TView, TModel>(TModel model)
            where TPresenter : IWorldObjectPresenter<TView, TModel>
            where TView : IWorldObjectView
            where TModel : IModel;

        void Release<TView, TModel>(IWorldObjectPresenter<TView, TModel> presenter)
            where TModel : IModel
            where TView : IWorldObjectView;
    }
}