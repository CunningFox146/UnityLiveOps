using System.Threading;
using CunningFox.LiveOps.Models;
using Cysharp.Threading.Tasks;

namespace Common.Api
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