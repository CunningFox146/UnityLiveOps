using System.Net.Http;
using System.Text;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Core.Services.Api
{
    public class UnityHttpClient : IHttpClient
    {
        private readonly string _baseUrl;

        public UnityHttpClient(string baseUrl = "")
        {
            _baseUrl = baseUrl;
        }

        public async UniTask<string> GetStringAsync(string url, CancellationToken cancellationToken = default)
        {
            using var request = UnityWebRequest.Get(BuildUrl(url));
            return await SendRequestAsync(request, cancellationToken);
        }

        public async UniTask<byte[]> GetBytesAsync(string url, CancellationToken cancellationToken = default)
        {
            using var request = UnityWebRequest.Get(BuildUrl(url));
            await SendRequestAsync(request, cancellationToken);
            return request.downloadHandler.data;
        }

        public async UniTask<string> PostAsync(string url, string jsonBody,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateJsonRequest(BuildUrl(url), ApiConstants.Post, jsonBody);
            return await SendRequestAsync(request, cancellationToken);
        }

        public async UniTask<string> PutAsync(string url, string jsonBody,
            CancellationToken cancellationToken = default)
        {
            using var request = CreateJsonRequest(BuildUrl(url), ApiConstants.Put, jsonBody);
            return await SendRequestAsync(request, cancellationToken);
        }

        public async UniTask<string> DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            using var request = UnityWebRequest.Delete(BuildUrl(url));
            request.downloadHandler = new DownloadHandlerBuffer();
            return await SendRequestAsync(request, cancellationToken);
        }

        private static UnityWebRequest CreateJsonRequest(string url, string method, string jsonBody)
        {
            var request = new UnityWebRequest(url, method)
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonBody)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader(ApiConstants.ContentTypeHeader, ApiConstants.JsonContentType);
            return request;
        }

        private static async UniTask<string> SendRequestAsync(UnityWebRequest request, CancellationToken ct)
        {
            await request.SendWebRequest().ToUniTask(cancellationToken: ct);
            return request.result is UnityWebRequest.Result.Success
                ? request.downloadHandler.text
                : throw new HttpRequestException(ZString.Concat(request.error, request.responseCode));
        }

        private string BuildUrl(string url)
            => ZString.Concat(_baseUrl, "/", url);
    }
}