using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public class StateMachine<T> : IStateMachine<T> where T : class
    {
        protected Dictionary<string, IState<T>> m_states = new Dictionary<string, IState<T>>();
        protected bool m_active;
        protected string m_currStateName;
        protected string m_nextStateName;
        protected IState<T> m_curState;
        protected T m_obj;

        protected enum EnumState
        {
            Normal = 0,
            Changing,
        }
        protected EnumState m_state;

        public void AddState<STATE>(string name, STATE state) where STATE : IState<T>
        {
            if (m_states.ContainsKey(name))
                return;
            state.InitStateMachine(this);
            m_states.Add(name, state);
        }

        public void Start(T obj, string initialState)
        {
            if (m_active)
                return;
            m_active = true;
            m_obj = obj;
            m_currStateName = string.Empty;
            m_nextStateName = initialState;
            m_state = EnumState.Normal;
        }

        public void Update(float elapsedTime)
        {
            if (!m_active)
                return;
            if (m_state == EnumState.Normal)
            {
                if (m_curState != null)
                {
                    m_curState.OnUpdate(m_obj, elapsedTime);
                }
                if (m_currStateName != m_nextStateName)
                {
                    IState<T> currState = m_curState;
                    IState<T> nextState = null;
                    if (!string.IsNullOrEmpty(m_nextStateName) && m_states.ContainsKey(m_nextStateName))
                    {
                        nextState = m_states[m_nextStateName];
                    }
                    ChangeStateAsync(currState, nextState, m_nextStateName).Forget();
                    m_state = EnumState.Changing;
                }
            }
        }

        public void ChangeState(string stateName)
        {
            m_nextStateName = stateName;
        }

        private async UniTaskVoid ChangeStateAsync(IState<T> currState, IState<T> nextState, string nextStateName)
        {
            if (currState != null)
            {
                await currState.OnLeave(m_obj);
            }
            if (nextState != null)
            {
                await nextState.OnEnter(m_obj);
            }
            m_currStateName = nextStateName;
            m_curState = nextState;
            m_state = EnumState.Normal;
        }
    }
}