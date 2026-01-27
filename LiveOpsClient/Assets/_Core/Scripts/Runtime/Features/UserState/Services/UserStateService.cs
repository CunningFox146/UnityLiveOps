using System.Threading;
using App.Runtime.Features.UserState.Models;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.UserState.Services
{
    public class UserStateService : IUserStateService
    {
        private readonly IRepository<ActiveUserState> _repository;

        private ActiveUserState Data => _repository.Value;
        public int CurrentLevel => _repository.Value.CurrentLevel;

        public UserStateService(IRepository<ActiveUserState> repository)
        {
            _repository = repository;
        }

        public UniTask RestoreUserState(CancellationToken token)
        {
            return _repository.RestoreFeatureData(token);
        }
        
        public void SetCurrentLevel(int level)
        {
            Data.CurrentLevel = level;
            _repository.Update(Data);
        }
    }
}