using System;

namespace App.Shared.Time
{
    public interface ITimeService
    {
        event Action<DateTime> OnTimeChanged;
        DateTime Now { get; }
    }
}