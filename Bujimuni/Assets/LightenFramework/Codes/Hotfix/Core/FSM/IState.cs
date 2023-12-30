
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public interface IState<T> where T : class
    {
        void InitStateMachine(IStateMachine<T> stateMachine);
        void ChangeState(string stateName);
        UniTask OnEnter(T obj);
        UniTask OnLeave(T obj);
        void OnUpdate(T obj, float elapsedTime);
    }
}