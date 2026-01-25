using System;

namespace Common
{
    public interface ICloseableView
    {
        event Action ViewClosed;
        void RequestClose();
    }
}
