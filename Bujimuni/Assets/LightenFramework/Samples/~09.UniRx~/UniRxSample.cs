using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public class UniRxSample : MonoBehaviour
{
    private RxProperty<int> m_life = new RxProperty<int>(1);
    private RxProperty<int> m_mama = new RxProperty<int>(1);

    private RxList<int> m_list = new RxList<int>();
    private RxDictionary<string, int> m_dict = new RxDictionary<string, int>();

    private List<int> m_list2 = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        this.Register();
        //m_dict.ObserveAdd().Subscribe(OnDictionaryAdd);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_life.Value = 0;
            m_mama.Value = 0;
            for (int i = 0; i < 5; ++i)
            {
                m_life.Value = m_life + 1;
                m_mama.Value = m_mama + 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.m_list.Clear();
            this.m_list.AddRange(new List<int>(){-1,-2,-3});
            Debug.Log($"m_list = {this.m_list}");
            this.m_list.Insert(2, 99);
            Debug.Log($"m_list = {this.m_list}");
            this.m_list.InsertRange(2, new List<int>(){-4,-5,-6});
            Debug.Log($"m_list = {this.m_list}");
            this.m_list.RemoveAt(0);
            Debug.Log($"m_list = {this.m_list}");
            this.m_list.RemoveAll(item => item == 2);
            Debug.Log($"m_list = {this.m_list}");
            this.m_list[1] = 100;
            Debug.Log($"m_list = {this.m_list}");
            //Debug.Log($"m_list =\n{this.m_list.ToStringWithSepartor("\n")}");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.m_dict.Clear();
            this.m_dict.Add("a", 1);
            Debug.Log($"m_dict = {this.m_dict}");
            this.m_dict.Add("b", 2);
            Debug.Log($"m_dict = {this.m_dict}");
            this.m_dict.Remove("a");
            Debug.Log($"m_dict = {this.m_dict}");
            this.m_dict["hello"] = 9527;
            Debug.Log($"m_dict = {this.m_dict}");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.UnRegister();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.Register();
        }
    }

    void Register()
    {
        this.m_life.Subscribe(v => Debug.Log($"life change to {v}")).AddTo(this);
        this.m_mama.Subscribe(v => Debug.Log($"mana change to {v}")).AddTo(this);
        this.m_list.OnAdded.Subscribe(e => Debug.Log($"OnCollectionAdd {e.Index} {e.Value}")).AddTo(this);
        this.m_list.OnRemoved.Subscribe(e => Debug.Log($"OnCollectionRemove {e.Index} {e.Value}")).AddTo(this);
        this.m_list.OnInserted.Subscribe(e => Debug.Log($"OnCollectionInsert {e.Index} {e.Value}")).AddTo(this);
        this.m_list.OnCountChanged.Subscribe(e => Debug.Log($"OnCollectionCountChanged {e.Count}")).AddTo(this);
        this.m_list.OnElementChanged.Subscribe(e => Debug.Log($"OnCollectionElementChanged {e.Index} {e.Value}")).AddTo(this);

        this.m_dict.OnAdded.Subscribe(e => Debug.Log($"OnDictionaryAdd {e.Key} {e.Value}")).AddTo(this);
        this.m_dict.OnRemoved.Subscribe(e => Debug.Log($"OnDictionaryRemove {e.Key} {e.Value}")).AddTo(this);
        this.m_dict.OnCountChanged.Subscribe(e => Debug.Log($"OnDictionaryCountChanged {e.Count}")).AddTo(this);
        this.m_dict.OnElementChanged.Subscribe(e => Debug.Log($"OnDictionaryElementChanged {e.Key} {e.Value}")).AddTo(this);
    }
    void UnRegister()
    {
        gameObject.UnRegisterAll();
    }
}