namespace App.Shared.Mvc.Factory
{
    public interface IViewControllerFactory
    {
        T Create<T>() where T : class;
    }
}