using System;
using System.Collections.Generic;

namespace Lighten
{
    public static class ObjectPool
    {
        private const int POOL_SIZE = 1000;
        private static Dictionary<Type, Stack<object>> m_pools = new Dictionary<Type, Stack<object>>();

        public static T Fetch<T>() where T : class, new()
        {
            var type = typeof(T);
            if (m_pools.ContainsKey(type))
            {
                var stack = m_pools[type];
                if (stack.Count > 0)
                {
                    return stack.Pop() as T;
                }
            }

            return new T();
        }

        public static object Fetch(Type type)
        {
            if (m_pools.ContainsKey(type))
            {
                var stack = m_pools[type];
                if (stack.Count > 0)
                {
                    return stack.Pop();
                }
            }

            return Activator.CreateInstance(type);
        }

        public static void Recycle(object obj)
        {
            var type = obj.GetType();
            Stack<object> stack;
            if (m_pools.ContainsKey(type))
            {
                stack = m_pools[type];
            }
            else
            {
                stack = new Stack<object>();
                m_pools.Add(type, stack);
            }

            if (stack.Count > POOL_SIZE)
            {
                return;
            }

            stack.Push(obj);
        }
    }
}