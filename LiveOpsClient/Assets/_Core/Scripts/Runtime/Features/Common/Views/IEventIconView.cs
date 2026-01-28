using System;

namespace App.Runtime.Features.Common.Views
{
    public interface IEventIconView : IDisposable
    {
        event Action Clicked;
        void SetTimeLeft(TimeSpan timeLeft);
        void Expire();
    }
}