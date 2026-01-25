using System.Threading;
using Cysharp.Threading.Tasks;

namespace Common.Repository
{
    public interface IReadOnlyRepository<TDto>
    {
        UniTask<TDto> Get(CancellationToken cancellationToken = default);
    }
}