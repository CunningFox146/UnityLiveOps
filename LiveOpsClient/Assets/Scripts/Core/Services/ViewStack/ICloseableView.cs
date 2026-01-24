using System;

namespace Core.Services.Views
{
    public interface ICloseableView
    {
        event Action ViewClosed;
        void RequestClose();
    }
}
