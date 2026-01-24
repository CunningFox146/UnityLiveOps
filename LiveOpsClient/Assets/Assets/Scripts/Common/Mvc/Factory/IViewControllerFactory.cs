namespace Common.Mvc.Factory
{
    public interface IViewControllerFactory
    {
        T Create<T>() where T : class;
    }
}