using System;

namespace Lighten
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>, IDisposable, new ()
    {

        private static T m_Instance;
        private static object m_lock = new object();

        public static T Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = SingletonCreator.CreateSingleton<T>();
                    }

                    return m_Instance;
                }
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
            m_Instance = null;
        }
    }
}
