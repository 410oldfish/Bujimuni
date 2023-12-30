using System;
using System.Reflection;

namespace Lighten
{
    [MonoSingletonPath("LightenFramework/AssemblyRunner")]
    public class AssemblyRunner : MonoSingleton<AssemblyRunner>
    {
        private Assembly m_assembly;
        private MethodInfo m_start;
        private MethodInfo m_update;
        private MethodInfo m_finish;
        private bool m_running;
        
        private void Update()
        {
            if (m_update != null)
            {
                m_update.Invoke(null, null);
            }
        }

        public bool IsRunning()
        {
            return m_running;
        }
        public Assembly GetRunningAssembly()
        {
            return m_assembly;
        }
        
        public void Run(Assembly assembly)
        {
            m_assembly = assembly;
            Type gameMain = assembly.GetType("GameMain");
            m_start = gameMain.GetMethod("Start");
            m_update = gameMain.GetMethod("Update");
            m_finish = gameMain.GetMethod("Finish");
            if (m_start != null)
            {
                m_start.Invoke(null, null);
            }
            m_running = true;
        }
        
        public void Stop()
        {
            if (m_finish != null)
            {
                m_finish.Invoke(null, null);
            }
            m_running = false;
        }
    }
}
