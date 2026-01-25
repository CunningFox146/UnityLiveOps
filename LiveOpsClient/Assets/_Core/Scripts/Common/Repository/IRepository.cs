using System;

namespace Common.Repository
{
    public interface IRepository<TDto> : IReadOnlyRepository<TDto>
    {
        void Update(TDto data);
        event Action RepositoryUpdated;
    }
}