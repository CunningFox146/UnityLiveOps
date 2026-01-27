using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Runtime.Features.UserState.Services
{
    public interface IUserStateService
    {
        int CurrentLevel { get; }
        UniTask RestoreUserState(CancellationToken token);
        void SetCurrentLevel(int level);
    }
}