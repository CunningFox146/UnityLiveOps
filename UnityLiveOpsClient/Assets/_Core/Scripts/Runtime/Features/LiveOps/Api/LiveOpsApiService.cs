using System.Threading;
using App.Shared.Api;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Api
{
    public class LiveOpsApiService : ApiServiceBase, ILiveOpsApiService
    {
        public LiveOpsApiService(IHttpClient httpClient) : base(httpClient) { }

        public UniTask<LiveOpsCalendarDto> GetCalendar(CancellationToken token = default)
            => GetAsync<LiveOpsCalendarDto>("LiveOps/Active", token);

        public UniTask<string> GetCalendarId(CancellationToken token = default)
            => GetStringAsync("LiveOps/ActiveId", token);
    }
}