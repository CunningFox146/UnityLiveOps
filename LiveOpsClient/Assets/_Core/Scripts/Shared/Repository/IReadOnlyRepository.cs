using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Repository
{
    public interface IReadOnlyRepository<TDto>
    {
        UniTask<TDto> Get(CancellationToken cancellationToken = default);
    }
}