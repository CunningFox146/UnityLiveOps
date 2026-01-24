using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CunningFox.Repository
{
    public interface IRepository<TDto> : IReadOnlyRepository<TDto>
    {
        void Update(TDto data);
        event Action RepositoryUpdated;
    }
}