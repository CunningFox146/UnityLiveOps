using System;
using System.Threading;
using Core.Infrastructure.Logger;
using Core.Infrastructure.Storage;
using Cysharp.Threading.Tasks;

namespace CunningFox.Repository
{
    public abstract class FeatureRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly IPersistentStorage _persistentStorage;
        private readonly ILogger _logger;
        private readonly string _key;
        private readonly SemaphoreSlim _semaphore;
        private readonly T _defaultValue;
        private T _lastValue;
        public event Action RepositoryUpdated;

        protected FeatureRepository(
            IPersistentStorage persistentStorage,
            ILogger logger,
            string key,
            T defaultValue = null)
        {
            _persistentStorage = persistentStorage;
            _logger = logger;
            _key = key;
            _defaultValue = defaultValue ?? new T();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public virtual async UniTask<T> Get(CancellationToken cancellationToken = default)
        {
            _lastValue ??= await RestoreFeatureData(cancellationToken);
            return _lastValue;
        }

        private async UniTask<T> RestoreFeatureData(CancellationToken cancellationToken = default)
        {
            try
            {
                var data = await _persistentStorage.LoadAsync<T>(_key, cancellationToken);
                return data;
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to restore persistent data", exception);
            }

            return _defaultValue;
        }

        public void Update(T data)
        {
            UpdateInternal(data).Forget();
        }

        private async UniTask UpdateInternal(T data)
        {
            await _semaphore.WaitAsync().AsUniTask();

            try
            {
                _lastValue = data;
                await _persistentStorage.SaveAsync(_key, _lastValue);
                RepositoryUpdated?.Invoke();
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to store persistent data", exception);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}