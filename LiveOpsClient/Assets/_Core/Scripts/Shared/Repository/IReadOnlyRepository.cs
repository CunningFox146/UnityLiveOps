using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Repository
{
    public interface IReadOnlyRepository<out TDto>
    {
        TDto Value { get; }
        UniTask RestoreFeatureData(CancellationToken cancellationToken = default);
    }
}