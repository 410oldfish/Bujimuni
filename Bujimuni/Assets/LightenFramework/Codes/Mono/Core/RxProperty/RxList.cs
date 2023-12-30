using System;
using System.Collections.Generic;
using System.Linq;

namespace Lighten
{
    #region 事件定义
    
    //重置列表
    public struct RxListClearEvent
    {
    }

    //添加元素
    public struct RxListAddEvent<T>
    {
        public int Index { get; private set; }
        public T Value { get; private set; }

        public RxListAddEvent(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    //移除元素
    public struct RxListRemoveEvent<T>
    {
        public int Index { get; private set; }
        public T Value { get; private set; }

        public RxListRemoveEvent(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    //插入元素
    public struct RxListInsertEvent<T>
    {
        public int Index { get; private set; }
        public T Value { get; private set; }

        public RxListInsertEvent(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    //数量改变
    public struct RxListCountChangedEvent
    {
        public int Count { get; private set; }

        public RxListCountChangedEvent(int count)
        {
            this.Count = count;
        }
    }

    //元素修改
    public struct RxListElementChangedEvent<T>
    {
        public int Index { get; private set; }
        public T Value { get; private set; }

        public RxListElementChangedEvent(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    #endregion

    public class RxList<T> : List<T>
    {
        public RxObservable<RxListClearEvent> OnCleared { get; private set; } = new();
        public RxObservable<RxListAddEvent<T>> OnAdded { get; private set; } = new();
        public RxObservable<RxListRemoveEvent<T>> OnRemoved { get; private set; } = new();
        public RxObservable<RxListInsertEvent<T>> OnInserted { get; private set; } = new();
        public RxObservable<RxListCountChangedEvent> OnCountChanged { get; private set; } = new();
        public RxObservable<RxListElementChangedEvent<T>> OnElementChanged { get; private set; } = new();

        private List<(int, T)> m_removedList = new List<(int, T)>();

        public new void Clear()
        {
            var oldCount = this.Count;
            base.Clear();
            OnCleared.Notify(new RxListClearEvent());
            if (oldCount > 0)
            {
                OnCountChanged.Notify(new RxListCountChangedEvent(0));
            }
        }
        public new void Add(T item)
        {
            base.Add(item);
            OnAdded.Notify(new RxListAddEvent<T>(this.Count - 1, item));
            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            int oldCount = this.Count;
            base.AddRange(collection);
            foreach (var newItem in collection)
            {
                OnAdded.Notify(new RxListAddEvent<T>(oldCount++, newItem));
            }

            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnInserted.Notify(new RxListInsertEvent<T>(index, item));
            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }
        
        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            foreach (var newItem in collection)
            {
                OnInserted.Notify(new RxListInsertEvent<T>(index++, newItem));
            }

            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }

        public new void Remove(T item)
        {
            var index = this.IndexOf(item);
            base.Remove(item);
            OnRemoved.Notify(new RxListRemoveEvent<T>(index, item));
            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }
        
        public new void RemoveRange(int index, int count)
        {
            m_removedList.Clear();
            for (int i = index, max = index + count; i < max; i++)
            {
                m_removedList.Add((i, this[i]));
            }
            base.RemoveRange(index, count);
            foreach (var removedItem in m_removedList)
            {
                OnRemoved.Notify(new RxListRemoveEvent<T>(removedItem.Item1, removedItem.Item2));
            }
            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }

        public new void RemoveAt(int index)
        {
            var item = this[index];
            base.RemoveAt(index);
            OnRemoved.Notify(new RxListRemoveEvent<T>(index, item));
            OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
        }

        public new void RemoveAll(Predicate<T> match)
        {
            m_removedList.Clear();
            for (int i = 0; i < this.Count; i++)
            {
                var item = this[i];
                if (match(item))
                {
                    m_removedList.Add((i, item));
                }
            }
            base.RemoveAll(match);
            if (m_removedList.Count > 0)
            {
                foreach (var removedItem in m_removedList)
                {
                    OnRemoved.Notify(new RxListRemoveEvent<T>(removedItem.Item1, removedItem.Item2));
                }
                OnCountChanged.Notify(new RxListCountChangedEvent(this.Count));
            }
        }
        
        public new T this[int index]
        {
            get => base[index];
            set
            {
                var oldValue = base[index];
                if (EqualityComparer<T>.Default.Equals(oldValue, value))
                    return;
                base[index] = value;
                OnElementChanged.Notify(new RxListElementChangedEvent<T>(index, value));
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
            for (int i = 0; i < this.Count; i++)
            {
                if (i == 0)
                {
                    text += $"[{i}]:{this[i].ToString()}";
                }
                else
                {
                    text += $"{separtor}[{i}]:{this[i].ToString()}";
                }
            }

            return text;
        }
    }
}