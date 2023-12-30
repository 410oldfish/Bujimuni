using System.Collections;
using System.Collections.Generic;
using Lighten;
using UnityEngine;

public class XEntitySample : MonoBehaviour
{
    public class C : XEntity, IAwake<int>, IUpdate, IDestroy
    {
        public int Value;
        private bool m_firstUpdate;

        public void Awake(int id)
        {
            this.Value = id;
            m_firstUpdate = true;
            Debug.Log($"Awake C{this.Value}");
        }

        public void Update(float elapsedTime)
        {
            if (m_firstUpdate)
            {
                Debug.Log($"Update C{this.Value}");
                m_firstUpdate = false;
            }
        }

        public void Destroy()
        {
            Debug.Log($"Destroy C{this.Value}");
        }
    }

    public class D : XEntity, IAwake<int>, IUpdatePerSecond, IDestroy
    {
        public int Value;

        public void Awake(int id)
        {
            this.Value = id;
            Debug.Log($"Awake D{this.Value}");
        }

        public void UpdatePerSecond()
        {
            Debug.Log($"Update D{this.Value}");
        }

        public void Destroy()
        {
            Debug.Log($"Destroy D{this.Value}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var c1 = Game.Root.AddChild<C, int>(1);
            c1.AddComponent<C, int>(101);
            c1.AddComponent<D, int>(102);
            var c2 = Game.Root.AddChild<C, int>(2);
            c2.AddChild<C, int>(21);
            c2.AddChild<C, int>(22);
            var c3 = Game.Root.AddChild<C, int>(3);
            c3.AddComponent<C, int>(301);
            c3.AddComponent<D, int>(302);
            c3.AddChild<C, int>(31);
            c3.AddChild<C, int>(32);
        }
    }
}