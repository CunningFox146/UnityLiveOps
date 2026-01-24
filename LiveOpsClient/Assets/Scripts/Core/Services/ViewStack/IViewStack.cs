namespace Core.Services.Views
{
    public interface IViewStack
    {
        void Push(ICloseableView view);
        void Pop();
    }
}
