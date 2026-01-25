namespace Common
{
    public interface IViewStack
    {
        void Push(ICloseableView view);
        void Pop();
    }
}
