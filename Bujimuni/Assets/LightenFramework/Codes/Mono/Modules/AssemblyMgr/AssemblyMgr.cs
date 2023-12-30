using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public interface IAssemblyMgr
    {
        UniTask Initialize();
        Type GetType(string typeName);
        void Run();
        void Stop();
    }

    public class AssemblyMgr : AbstractManager, IAssemblyMgr, IAwake, IUpdate
    {
        private Dictionary<string, Assembly> m_assemblies = new Dictionary<string, Assembly>();
        private Dictionary<string, Type> m_typeDict = new Dictionary<string, Type>();

        private List<string> m_assemblyNames = new List<string>()
        {
            "Lighten.Hotfix", "Lighten.Mono", "Game.Hotfix", "Game.Mono"
        };
        
        private MethodInfo m_start;
        private MethodInfo m_update;
        private MethodInfo m_finish;
        private bool m_running;

        public void Awake()
        {
            using (TimeMonitor.Start("Assembly init"))
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    if (m_assemblyNames.Contains(assembly.GetName().Name))
                    {
                        this.RegisterAssembly(assembly);
                    }
                }
            }
        }
        
        public void Update(float elapsedTime)
        {
            if (m_update != null)
            {
                m_update.Invoke(null, null);
            }
        }

        private void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (string.IsNullOrEmpty(type.FullName))
                    continue;
                this.m_typeDict[type.FullName] = type;
            }

            m_assemblies[assembly.FullName] = assembly;
        }

        //根据类型名称查找类型
        public async UniTask Initialize()
        {
            await UniTask.CompletedTask;
        }
        
        public void Run()
        {
            var assemblyMgr = Game.Architecture.GetManager<IAssemblyMgr>();
            Type gameMain = assemblyMgr.GetType("GameMain");
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

        public Type GetType(string typeName)
        {
            if (this.m_typeDict.TryGetValue(typeName, out var type))
            {
                return type;
            }

            return null;
        }

        //根据Atrribute查找所有的类型
        public Type[] GetTypesByAttribute<T>(bool inherit = false) where T : Attribute
        {
            var attributes = new List<Type>();
            foreach (var type in m_typeDict.Values)
            {
                if (type.IsDefined(typeof(T), inherit))
                {
                    attributes.Add(type);
                }
            }

            return attributes.ToArray();
        }

        
    }
}