namespace App.Runtime.Services.ViewStack
{
    public interface IViewStack
    {
        public int ViewsCount { get; }
        public ICloseableView TopView { get; }
        
        void Push(ICloseableView view);
        void Pop(bool isClosing = true);
    }
}
