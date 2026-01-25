using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Api
{
    public interface IHttpClient
    {
        UniTask<string> GetStringAsync(string url, CancellationToken cancellationToken = default);
        UniTask<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default);
        UniTask<string> PostAsync(string url, string jsonBody, CancellationToken cancellationToken = default);
        UniTask<string> PutAsync(string url, string jsonBody, CancellationToken cancellationToken = default);
    }
}
