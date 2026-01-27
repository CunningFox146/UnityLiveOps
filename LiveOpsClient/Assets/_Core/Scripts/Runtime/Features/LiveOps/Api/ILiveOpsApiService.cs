using System;
using System.Threading;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public interface ILiveOpsApiService
    {
        UniTask<LiveOpsCalendarDto> GetCalendar(CancellationToken token = default);
        UniTask<Guid> GetCalendarId(CancellationToken token = default);
    }
}