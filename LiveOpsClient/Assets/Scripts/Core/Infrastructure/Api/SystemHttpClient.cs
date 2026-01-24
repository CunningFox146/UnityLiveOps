using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Services.Api
{
    public class SystemHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public SystemHttpClient(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async UniTask<string> GetStringAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async UniTask<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async UniTask<string> PostAsync(string url, string jsonBody,
            CancellationToken cancellationToken = default)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, ApiConstants.JsonContentType);
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async UniTask<string> PutAsync(string url, string jsonBody,
            CancellationToken cancellationToken = default)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, ApiConstants.JsonContentType);
            var response = await _httpClient.PutAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}