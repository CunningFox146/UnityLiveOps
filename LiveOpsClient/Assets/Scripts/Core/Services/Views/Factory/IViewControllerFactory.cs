namespace Core.Core.Services.Views
{
    public interface IViewControllerFactory
    {
        T Create<T>() where T : class, IViewController;
        T Create<T, TResult, TInput>() where T : class, IViewControllerWithResult<TResult, TInput>;
    }
}