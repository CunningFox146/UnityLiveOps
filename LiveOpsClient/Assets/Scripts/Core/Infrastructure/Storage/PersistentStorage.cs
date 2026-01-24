using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Infrastructure.Storage
{
    public sealed class PersistentStorage : IPersistentStorage, IDisposable
    {
        private const int BufferSize = 4096;
        private const string FileExtension = ".dat";
        private const string TempFileExtension = ".tmp";

        private readonly string _basePath;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new();

        public PersistentStorage(string basePath)
        {
            _basePath = basePath;

            _serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public async UniTask SaveAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            await UniTask.SwitchToThreadPool();
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = GetFilePath(key);
            var tempPath = ZString.Concat(filePath, TempFileExtension);
            var fileLock = GetFileSemaphore(filePath);

            await fileLock.WaitAsync(cancellationToken);

            try
            {
                EnsureDirectoryExists(Path.GetDirectoryName(filePath));
                await WriteToTempFileAsync(tempPath, data, cancellationToken);
                ReplaceFile(filePath, tempPath);
            }
            finally
            {
                DeleteFileIfExists(tempPath);
                fileLock.Release();
            }
        }

        public async UniTask<T> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            await UniTask.SwitchToThreadPool();
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = GetFilePath(key);
            var fileLock = GetFileSemaphore(filePath);

            await fileLock.WaitAsync(cancellationToken);

            try
            {
                if (!File.Exists(filePath))
                    return default;

                return await ReadFromFileAsync<T>(filePath, cancellationToken);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public async UniTask DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            await UniTask.SwitchToThreadPool();
            cancellationToken.ThrowIfCancellationRequested();

            var filePath = GetFilePath(key);
            var fileLock = GetFileSemaphore(filePath);

            await fileLock.WaitAsync(cancellationToken);

            try
            {
                DeleteFileIfExists(filePath);
            }
            finally
            {
                fileLock.Release();
            }
        }

        public void Dispose()
        {
            foreach (var semaphore in _fileLocks.Values)
                semaphore.Dispose();

            _fileLocks.Clear();
        }

        private async UniTask WriteToTempFileAsync<T>(string tempPath, T data, CancellationToken cancellationToken)
        {
            var json = JsonConvert.SerializeObject(data, _serializerSettings);
            var bytes = Encoding.UTF8.GetBytes(json);

            using var stream = new FileStream(
                tempPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                BufferSize,
                true);

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await stream.FlushAsync(cancellationToken);
        }

        private async UniTask<T> ReadFromFileAsync<T>(string filePath, CancellationToken cancellationToken)
        {
            using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                BufferSize,
                true);

            var length = (int)stream.Length;
            var buffer = ArrayPool<byte>.Shared.Rent(length);

            try
            {
                var bytesRead = await stream.ReadAsync(buffer, 0, length, cancellationToken);

                if (bytesRead != length)
                    throw new IOException(
                        $"Failed to read file '{filePath}': expected {length} bytes, got {bytesRead}");

                var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private static void ReplaceFile(string targetPath, string sourcePath)
        {
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            File.Move(sourcePath, targetPath);
        }

        private static void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private string GetFilePath(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException("Invalid key", nameof(key));

            return Path.Combine(_basePath, ZString.Concat(key, FileExtension));
        }

        private SemaphoreSlim GetFileSemaphore(string filePath)
        {
            return _fileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
        }
    }
}