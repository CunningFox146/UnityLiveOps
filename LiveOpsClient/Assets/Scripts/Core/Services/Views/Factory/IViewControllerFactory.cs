namespace Core.Core.Services.Views
{
    public interface IViewControllerFactory
    {
        T Create<T>() where T : class;
    }
}