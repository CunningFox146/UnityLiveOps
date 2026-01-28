using System;

namespace App.Shared.Time
{
    public interface ITimeService
    {
        event Action<DateTime> TimeChanged;
        DateTime Now { get; }
    }
}