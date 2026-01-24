using System;

namespace BestFiends.Features.Album
{
    public interface IViewStack
    {
        void Push(ICloseableView view);
        void Pop();
    }
}
