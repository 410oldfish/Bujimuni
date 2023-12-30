using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class Pool<T> : IEnumerable<T> where T : class
    {
        public int Count => m_objects.Count;

        private Queue<T> m_objects = new Queue<T>();
        private Func<T> m_generator;
        private Action<T> m_onObjectSpawn;
        private Action<T> m_onObjectRecycle;
        private int m_capacity;

        public new string ToString()
        {
            return $"capacity:{m_capacity} count:{Count}";
        }

        public Pool(int capacity, Func<T> generator, Action<T> onObjectSpawn, Action<T> onObjectRecycle)
        {
            m_generator = generator;
            m_onObjectSpawn = onObjectSpawn;
            m_onObjectRecycle = onObjectRecycle;
            m_capacity = capacity;
        }
        public Pool(int capacity, Func<T> generator)
            : this(capacity, generator, null, null)
        {
        }
        public Pool(Func<T> generator)
            : this(0, generator, null, null)
        {
        }

        public void Clear()
        {
            m_objects.Clear();
        }

        public T Spawn()
        {
            T obj = null;
            if (m_objects.Count > 0)
            {
                obj = m_objects.Dequeue();
            }
            else
            {
                obj = m_generator();
            }
            m_onObjectSpawn?.Invoke(obj);
            return obj;
        }

        public void Recycle(T obj)
        {
            if (obj == null)
                return;
            if (m_capacity > 0 && m_objects.Count >= m_capacity)
                return;
            if (m_objects.Contains(obj))
                return;
            m_onObjectRecycle?.Invoke(obj);
            m_objects.Enqueue(obj);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
