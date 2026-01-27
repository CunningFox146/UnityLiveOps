namespace App.Shared.Mvc.Factory
{
    public interface IControllerFactory
    {
        T Create<T>() where T : class;
    }
}