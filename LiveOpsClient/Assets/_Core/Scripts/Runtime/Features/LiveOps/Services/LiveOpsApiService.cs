using System.Threading;
using App.Shared.Api;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.LiveOps.Services
{
    public class LiveOpsApiService : ApiServiceBase
    {
        public LiveOpsApiService(IHttpClient httpClient) : base(httpClient)
        {
        }

        public async UniTask<LiveOpCalendarDto> GetCalendar(CancellationToken cancellationToken = default)
        {
            return await GetAsync<LiveOpCalendarDto>("LiveOps/active", cancellationToken);
        }
    }
}