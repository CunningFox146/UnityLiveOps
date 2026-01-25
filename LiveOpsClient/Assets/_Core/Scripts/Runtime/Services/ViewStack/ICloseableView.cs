using System;

namespace App.Runtime.Services.ViewStack
{
    public interface ICloseableView
    {
        event Action ViewClosed;
        void RequestClose();
    }
}
