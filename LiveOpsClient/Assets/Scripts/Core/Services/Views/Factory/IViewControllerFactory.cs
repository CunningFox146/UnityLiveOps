namespace Core.Core.Services.Views
{
    public interface IViewControllerFactory
    {
        T Create<T, TResult, TInput>() where T : class, IViewController<TResult, TInput>;
    }
}