using System.Threading;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Api
{
    public interface ILiveOpsApiService
    {
        UniTask<LiveOpsCalendarDto> GetCalendar(CancellationToken token = default);
        UniTask<string> GetCalendarId(CancellationToken token = default);
    }
}