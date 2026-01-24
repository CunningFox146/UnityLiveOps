using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Infrastructure.Storage
{
    public interface IPersistentStorage
    {
        UniTask SaveAsync<T>(string key, T data, CancellationToken cancellationToken = default);
        UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default);
        void Delete(string key);
    }
}