using System.Threading;
using App.Runtime.Services.UserState;
using App.Shared.Repository;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.UserState.Service
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