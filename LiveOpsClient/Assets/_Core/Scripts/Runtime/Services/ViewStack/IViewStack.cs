namespace App.Runtime.Services.ViewStack
{
    public interface IViewStack
    {
        void Push(ICloseableView view);
        void Pop();
    }
}
