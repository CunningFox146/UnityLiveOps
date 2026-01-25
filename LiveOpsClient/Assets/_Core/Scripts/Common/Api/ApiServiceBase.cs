using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Common.Api
{
    public abstract class ApiServiceBase
    {
        private readonly IHttpClient _httpClient;

        protected ApiServiceBase(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async UniTask<T> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            var json = await GetStringAsync(url, cancellationToken);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected UniTask<string> GetStringAsync(string url, CancellationToken cancellationToken = default)
        {
            return _httpClient.GetStringAsync(url, cancellationToken);
        }

        protected UniTask<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default)
        {
            return _httpClient.GetBytesAsync(url, cancellationToken);
        }

        protected async UniTask<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest body,
            CancellationToken cancellationToken = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await _httpClient.PostAsync(url, jsonBody, cancellationToken);
            return JsonConvert.DeserializeObject<TResponse>(response);
        }

        protected async UniTask PostAsync<TRequest>(string url, TRequest body,
            CancellationToken cancellationToken = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            await _httpClient.PostAsync(url, jsonBody, cancellationToken);
        }

        protected async UniTask<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest body,
            CancellationToken cancellationToken = default)
        {
            var jsonBody = JsonConvert.SerializeObject(body);
            var response = await _httpClient.PutAsync(url, jsonBody, cancellationToken);
            return JsonConvert.DeserializeObject<TResponse>(response);
        }
    }
}