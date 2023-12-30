using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    #region 事件定义
    
    //重置字典
    public struct RxDictClearEvent
    {
    }
    
    //添加元素
    public struct RxDictAddEvent<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public RxDictAddEvent(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    
    //移除元素
    public struct RxDictRemoveEvent<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public RxDictRemoveEvent(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    
    //数量改变
    public struct RxDictCountChangedEvent
    {
        public int Count { get; private set; }

        public RxDictCountChangedEvent(int count)
        {
            this.Count = count;
        }
    }
    
    //元素修改
    public struct RxDictElementChangedEvent<TKey, TValue>
    {
        public TKey Key { get; private set; }
        public TValue Value { get; private set; }

        public RxDictElementChangedEvent(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    
    
    #endregion
    
    public class RxDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public RxObservable<RxDictClearEvent> OnCleared { get; private set; } = new();
        public RxObservable<RxDictAddEvent<TKey, TValue>> OnAdded { get; private set; } = new();
        public RxObservable<RxDictRemoveEvent<TKey, TValue>> OnRemoved { get; private set; } = new();
        public RxObservable<RxDictCountChangedEvent> OnCountChanged { get; private set; } = new();
        public RxObservable<RxDictElementChangedEvent<TKey, TValue>> OnElementChanged { get; private set; } = new();
        
        public new void Clear()
        {
            var oldCount = this.Count;
            base.Clear();
            if (oldCount > 0)
            {
                OnCountChanged.Notify(new RxDictCountChangedEvent(0));
            }
        }
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnAdded.Notify(new RxDictAddEvent<TKey, TValue>(key, value));
            OnCountChanged.Notify(new RxDictCountChangedEvent(this.Count));
        }
        public new void Remove(TKey key)
        {
            if (!TryGetValue(key, out var item))
            {
                return;
            }
            base.Remove(key);
            OnRemoved.Notify(new RxDictRemoveEvent<TKey, TValue>(key, item));
            OnCountChanged.Notify(new RxDictCountChangedEvent(this.Count));
        }
        
        public new TValue this[TKey key]
        {
            get => base[key];
            set
            {
                TryGetValue(key, out var oldValue);
                if (EqualityComparer<TValue>.Default.Equals(oldValue, value))
                    return;
                base[key] = value;
                OnElementChanged.Notify(new RxDictElementChangedEvent<TKey, TValue>(key, value));
            }
        }
        
        public override string ToString()
        {
            return this.ToStringWithSepartor(", ");
        }

        public string ToStringWithSepartor(string separtor)
        {
            if (this.Count <= 0)
            {
                return $"{base.ToString()} Empty!!";
            }

            var text = string.Empty;
            foreach (var kv in this)
            {
                if (string.IsNullOrEmpty(text))
                {
                    text += $"[{kv.Key}]:{kv.Value.ToString()}";
                }
                else
                {
                    text += $"{separtor}[{kv.Key}]:{kv.Value.ToString()}";
                }
            }
            
            return text;
        }
    }
}
