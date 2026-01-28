using System;
using System.Threading;
using App.Shared.Logger;
using App.Shared.Storage;
using Cysharp.Threading.Tasks;

namespace App.Shared.Repository
{
    public abstract class FeatureRepository<T> : IRepository<T> where T : class, new()
    {
        public event Action RepositoryUpdated;
        
        private readonly IPersistentStorage _persistentStorage;
        private readonly ILogger _logger;
        private readonly string _key;
        private readonly SemaphoreSlim _semaphore;
        private readonly T _defaultValue;
        
        public virtual T Value { get; private set; }

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
        
        public async UniTask RestoreFeatureData(CancellationToken token = default)
        {
            try
            {
                var result = await _persistentStorage.LoadAsync<T>(_key, token);
                Value = result ?? _defaultValue;
                return;
            }
            catch (Exception exception)
            {
                _logger.Error("Failed to restore persistent data", exception);
            }

            Value = _defaultValue;
        }

        public void Update(T data)
        {
            UpdateAsync(data).Forget();
        }

        public async UniTask UpdateAsync(T data, CancellationToken token = default)
        {
            await _semaphore.WaitAsync(token);

            try
            {
                Value = data;
                await _persistentStorage.SaveAsync(_key, Value, token);
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