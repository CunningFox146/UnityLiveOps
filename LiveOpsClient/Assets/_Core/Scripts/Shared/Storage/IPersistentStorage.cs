using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Storage
{
    public interface IPersistentStorage
    {
        UniTask SaveAsync<T>(string key, T data, CancellationToken token = default);
        UniTask<T> LoadAsync<T>(string key, CancellationToken token = default);
        void Delete(string key);
    }
}