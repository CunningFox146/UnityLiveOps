using System;

namespace BestFiends.Features.Album
{
    /// <summary>
    /// Use when popup should prevent back buttons click but shouldn't get closed by it
    /// </summary>
    public class ClosableViewPlaceHolder : ICloseableView
    {
        public event Action ViewClosed;

        public void RequestClose()
        {
        }
    }
}
