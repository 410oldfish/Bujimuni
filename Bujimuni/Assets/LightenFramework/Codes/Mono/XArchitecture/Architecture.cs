using System;
using System.Collections.Generic;

namespace Lighten
{
    public interface IArchitecture
    {
        void Init();
        void RegisterManager<T, K>() where K : XEntity, IManager, T;
        void RegisterManager<T>() where T : XEntity, IManager;
        void RegisterModel<T>() where T : XEntity, IModel;
        T GetManager<T>(bool throwException = true) where T : class;
        T GetModel<T>() where T : XEntity, IModel;
    }

    public abstract class AbstractArchitecture : XEntity, IArchitecture
    {
        private Dictionary<Type, Type> m_interfaceTypes = new Dictionary<Type, Type>();
        private bool m_inited;
        private Queue<Type> m_managerInitQueue = new Queue<Type>();

        public void Init()
        {
            if (m_inited)
                return;
            while (this.m_managerInitQueue.Count > 0)
            {
                var managerType = this.m_managerInitQueue.Dequeue();
                this.RegisterManagerInternal(managerType);
            }

            m_inited = true;
        }

        public void RegisterManager<TInterface, TManager>() where TManager : XEntity, IManager, TInterface
        {
            var managerType = typeof(TManager);
            m_interfaceTypes[typeof(TInterface)] = managerType;
            if (m_inited)
            {
                this.RegisterManagerInternal(managerType);
            }
            else
            {
                this.m_managerInitQueue.Enqueue(managerType);
            }
        }

        public void RegisterManager<TManager>() where TManager : XEntity, IManager
        {
            var managerType = typeof(TManager);
            if (m_inited)
            {
                this.RegisterManagerInternal(managerType);
            }
            else
            {
                this.m_managerInitQueue.Enqueue(managerType);
            }
        }

        private void RegisterManagerInternal(Type managerType)
        {
            var manager = this.AddComponent(managerType) as IManager;
            if (manager == null)
                return;
            manager.SetArchitecture(this);
        }

        public void RegisterModel<T>() where T : XEntity, IModel
        {
            var model = this.AddComponent<T>();
            model.SetArchitecture(this);
        }

        public T GetManager<T>(bool throwException = false) where T : class
        {
            var searchType = typeof(T);
            if (!this.m_interfaceTypes.TryGetValue(searchType, out var managerType))
            {
                managerType = searchType;
            }

            var manager = this.GetComponent(managerType);
            if (manager == null)
            {
                if (throwException)
                {
                    throw new Exception($"未注册Mgr {managerType.FullName}");
                }

                return null;
            }

            return manager as T;
        }

        public T GetModel<T>() where T : XEntity, IModel
        {
            var model = this.GetComponent<T>();
            if (model == null)
            {
                throw new Exception($"未注册Model {typeof(T).FullName}");
            }

            return model;
        }
    }

    public interface ICanGetArchitecture
    {
        IArchitecture GetArchitecture();
    }

    public interface ICanSetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }

    public abstract class AbstractGetSetArchitecture : XEntity, ICanGetArchitecture, ICanSetArchitecture
    {
        private IArchitecture m_architecture;

        IArchitecture ICanGetArchitecture.GetArchitecture()
        {
            return m_architecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            m_architecture = architecture;
        }
    }
}