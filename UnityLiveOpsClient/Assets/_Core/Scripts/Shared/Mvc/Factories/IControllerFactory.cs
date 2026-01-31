namespace App.Shared.Mvc.Factories
{
    public interface IControllerFactory
    {
        T Create<T>() where T : class;
    }
}