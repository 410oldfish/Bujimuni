
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public abstract class State<T> : IState<T> where T : class
    {
        private IStateMachine<T> m_stateMachine;
        public void InitStateMachine(IStateMachine<T> stateMachine)
        {
            m_stateMachine = stateMachine;
        }
        public void ChangeState(string stateName)
        {
            m_stateMachine.ChangeState(stateName);
        }
        
        public abstract UniTask OnEnter(T obj);
        public abstract UniTask OnLeave(T obj);
        public abstract void OnUpdate(T obj, float elapsedTime);
    }
}
