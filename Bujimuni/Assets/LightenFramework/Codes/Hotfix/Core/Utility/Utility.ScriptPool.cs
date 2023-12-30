using System;
using System.Collections.Generic;

namespace Lighten
{
    public static partial class Utility
    {
        public static class ScriptPool
        {
            private static Dictionary<Type, Pool<object>> m_pools = new Dictionary<Type, Pool<object>>();

            public static new string ToString()
            {
                var text = "===ScriptObjectPool===\n";
                foreach (var pairs in m_pools)
                {
                    text = text + $"{pairs.Key.Name} {pairs.Value.ToString()}\n";
                }

                return text;
            }

            public static T Spawn<T>() where T : class, new()
            {
                var type = typeof(T);
                Pool<object> pool = null;
                if (m_pools.ContainsKey(type))
                {
                    pool = m_pools[type];
                }
                else
                {
                    pool = new Pool<object>(() => { return new T(); });
                    m_pools.Add(type, pool);
                }

                return pool.Spawn() as T;
            }

            public static void Recycle<T>(T obj)
            {
                var type = typeof(T);
                //Debug.Log(type.FullName);
                if (m_pools.ContainsKey(type))
                {
                    m_pools[type].Recycle(obj);
                }
            }
        }
    }
}