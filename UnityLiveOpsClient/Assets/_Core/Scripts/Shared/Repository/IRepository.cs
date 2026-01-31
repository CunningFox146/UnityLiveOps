using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Shared.Repository
{
    public interface IRepository<TDto> : IReadOnlyRepository<TDto>
    {
        void Update(TDto data);
        void Clear();
        event Action RepositoryUpdated;
    }
}