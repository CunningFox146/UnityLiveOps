using System;

namespace BestFiends.Features.Album
{
    public interface ICloseableView
    {
        event Action ViewClosed;
        void RequestClose();
    }
}
