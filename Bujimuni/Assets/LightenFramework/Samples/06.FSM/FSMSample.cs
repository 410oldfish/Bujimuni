using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Lighten;
public class FSMSample : MonoBehaviour
{
    public class Char
    {
        public float a;
    }
    public class StateA : State<Char>
    {
        public async override UniTask OnEnter(Char obj)
        {
            Debug.Log($"enter State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"enter State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public async override UniTask OnLeave(Char obj)
        {
            Debug.Log($"leave State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"leave State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public override void OnUpdate(Char obj, float elapsedTime)
        {
            if (obj.a > 1)
            {
                ChangeState("StateB");
            }
        }
    }
    public class StateB : State<Char>
    {
        public async override UniTask OnEnter(Char obj)
        {
            Debug.Log($"enter State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"enter State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public async override UniTask OnLeave(Char obj)
        {
            Debug.Log($"leave State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"leave State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public override void OnUpdate(Char obj, float elapsedTime)
        {
            if (obj.a > 5)
            {
                ChangeState("StateC");
            }
        }
    }
    public class StateC : State<Char>
    {
        public async override UniTask OnEnter(Char obj)
        {
            Debug.Log($"enter State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"enter State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public async override UniTask OnLeave(Char obj)
        {
            Debug.Log($"leave State {this.ToString()} 11");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            Debug.Log($"leave State {this.ToString()} 22");
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        public override void OnUpdate(Char obj, float elapsedTime)
        {
            if (obj.a > 10)
            {
                ChangeState("StateA");
            }
        }
    }

    private Char m_char;
    private StateMachine<Char> m_stateMachine;
    
    // Start is called before the first frame update
    void Start()
    {
        m_char = new Char();
        m_stateMachine = new StateMachine<Char>();
        m_stateMachine.AddState("StateA", new StateA());
        m_stateMachine.AddState("StateB", new StateB());
        m_stateMachine.AddState("StateC", new StateC());
        m_stateMachine.Start(m_char, "StateA");
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.Update(Time.deltaTime);
        m_char.a += Time.deltaTime;
    }
}
