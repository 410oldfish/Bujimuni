#pragma warning disable ET0015

using System;
using UnityEngine;

namespace Lighten
{
    [AttributeUsage(AttributeTargets.Class)] //这个特性只能标记在Class上
    public class MonoSingletonPathAttribute : Attribute
    {
        public MonoSingletonPathAttribute(string pathInHierarchy)
        {
            PathInHierarchy = pathInHierarchy;
        }

        public string PathInHierarchy { get; private set; }
    }
    
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {

        private static T m_Instance;
        private static bool m_IsApplicationQuit = false;

        public static T Instance
        {
            get
            {
                if (m_Instance == null && !m_IsApplicationQuit)
                {
                    m_Instance = SingletonCreator.CreateMonoSingleton<T>();
                }

                return m_Instance;
            }
        }

        public virtual void OnSingletonInit()
        {
        }

        public virtual void OnSingletonDestroy()
        {
            
        }

        public void Dispose()
        {
            if (m_Instance == null)
                return;
            this.OnSingletonDestroy();
            Destroy(this.gameObject);
            m_Instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            m_IsApplicationQuit = true;
            this.Dispose();
        }

        protected virtual void OnDestroy()
        {
            this.Dispose();
        }
    }
}
