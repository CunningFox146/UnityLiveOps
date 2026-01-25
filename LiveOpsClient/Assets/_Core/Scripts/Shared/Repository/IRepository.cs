using System;

namespace App.Shared.Repository
{
    public interface IRepository<TDto> : IReadOnlyRepository<TDto>
    {
        void Update(TDto data);
        event Action RepositoryUpdated;
    }
}