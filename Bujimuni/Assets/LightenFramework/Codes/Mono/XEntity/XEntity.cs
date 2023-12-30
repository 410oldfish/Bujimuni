using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    #region 接口定义

    public interface IDomain
    {
        XEntity Domain { get; }
    }

    public interface IAwake
    {
        void Awake();
    }

    public interface IAwake<P1>
    {
        void Awake(P1 p1);
    }

    public interface IAwake<P1, P2>
    {
        void Awake(P1 p1, P2 p2);
    }

    public interface IDestroy
    {
        void Destroy();
    }

    public interface IUpdate
    {
        void Update(float elapsedTime);
    }

    public interface ILateUpdate
    {
        void LateUpdate(float elapsedTime);
    }

    public interface IFixedUpdate
    {
        void FixedUpdate(float elapsedTime);
    }

    public interface IUpdatePerSecond
    {
        void UpdatePerSecond();
    }

    #endregion

    public class XEntity : IDisposable
    {
        #region 回调事件
        
        //被销毁的回调
        public event Action OnDestroy;
        
        #endregion
        
        //唯一Id(创建后不会变化)
        public long Id { get; private set; }

        //实例Id(对象池回收再使用,会产生新的)
        public long InstanceId { get; private set; }

        //父节点
        public XEntity Parent { get; private set; }

        //子节点
        private Dictionary<long, XEntity> m_children;

        //组件
        private Dictionary<Type, XEntity> m_components;

        //是否已经销毁
        public bool IsDisposed => this.InstanceId == 0;

        //
        [Flags]
        private enum Status : byte
        {
            None = 0,
            IsFromPool = 1 << 0,
            IsComponent = 1 << 1,
            IsActived = 1 << 2,
        }

        private Status m_status;

        public bool IsFromPool
        {
            get => (this.m_status & Status.IsFromPool) != 0;
            private set => this.m_status =
                value ? this.m_status | Status.IsFromPool : this.m_status & ~Status.IsFromPool;
        }

        public bool IsComponent
        {
            get => (this.m_status & Status.IsComponent) != 0;
            private set => this.m_status =
                value ? this.m_status | Status.IsComponent : this.m_status & ~Status.IsComponent;
        }

        public bool IsActived
        {
            get => (this.m_status & Status.IsActived) != 0;
            private set
            {
                if (this.IsActived == value)
                    return;
                this.m_status = value ? this.m_status | Status.IsActived : this.m_status & ~Status.IsActived;
                if (value)
                {
                    this.InstanceId = IdGenerater.GenerateId();
                    XEntityMgr.Instance.Add(this);
                }
                else
                {
                    XEntityMgr.Instance.Remove(this.InstanceId);
                    this.InstanceId = 0;
                }
            }
        }

        protected XEntity()
        {
        }

        public void Dispose()
        {
            if (this.IsDisposed)
                return;
            this.IsActived = false;

            //销毁子节点
            if (this.m_children != null)
            {
                foreach (var child in this.m_children.Values)
                {
                    child.Dispose();
                }

                this.m_children.Clear();
                this.m_children = null;
            }

            //销毁组件
            if (this.m_components != null)
            {
                foreach (var kv in this.m_components)
                {
                    kv.Value.Dispose();
                }

                this.m_components.Clear();
                this.m_components = null;
            }

            //触发IDestroy
            (this as IDestroy)?.Destroy();
            this.OnDestroy?.Invoke();

            //从父节点移除自己
            if (this.Parent != null)
            {
                if (!this.Parent.IsDisposed)
                {
                    if (this.IsComponent)
                    {
                        this.Parent.RemoveComponent(this);
                    }
                    else
                    {
                        this.Parent.RemoveFromChildren(this);
                    }
                }

                this.Parent = null;
            }

            if (this.IsFromPool)
            {
                ObjectPool.Recycle(this);
            }

            this.m_status = Status.None;
        }

        public T GetParent<T>() where T : XEntity
        {
            return this.Parent as T;
        }

        public T FindInParent<T>() where T : XEntity
        {
            if (this.Parent is T result)
            {
                return result;
            }
            return this.Parent.FindInParent<T>();
        }

        public void SetParent(XEntity parent)
        {
            if (parent == null)
            {
                throw new Exception("不能设置空的父节点");
            }

            if (this.Parent == this)
            {
                throw new Exception("不能把自己设置为自己的父节点");
            }

            if (this.Parent != null)
            {
                if (this.Parent == parent)
                {
                    return;
                }

                this.Parent.RemoveFromChildren(this);
            }

            this.Parent = parent;
            this.IsComponent = false;
            this.IsActived = parent.IsActived;
            this.Parent.AddToChildren(this);
        }

        //创建根节点
        public static XEntity CreateRoot()
        {
            var entity = new XEntity();
            entity.Parent = entity;
            entity.IsActived = true;
            return entity;
        }

        //保证type是Entity的子类
        private XEntity CreateEntity(Type type)
        {
            var component = ObjectPool.Fetch(type) as XEntity;
            if (component == null)
            {
                throw new Exception($"type:{type.FullName} is not a XEntity");
            }

            component.IsFromPool = true;
            return component;
        }

        #region Component

        public T GetComponent<T>() where T : XEntity
        {
            var component = this.GetComponent(typeof(T));
            if (component == null)
                return null;
            return component as T;
        }

        public XEntity GetComponent(Type type)
        {
            if (this.m_components == null)
                return null;
            if (!this.m_components.TryGetValue(type, out var component))
                return null;
            return component;
        }

        public XEntity AddComponent(Type type)
        {
            if (this.HasComponent(type))
            {
                throw new Exception($"entity already has component: {type.FullName}");
            }

            var component = AddComponentInternal(type);
            (component as IAwake)?.Awake();
            return component;
        }

        public T AddComponent<T>() where T : XEntity
        {
            var type = typeof(T);
            return AddComponent(type) as T;
        }

        public T AddComponent<T, P1>(P1 p1) where T : XEntity, IAwake<P1>
        {
            var type = typeof(T);
            var component = AddComponent(type);
            (component as IAwake<P1>)?.Awake(p1);
            return component as T;
        }

        public T AddComponent<T, P1, P2>(P1 p1, P2 p2) where T : XEntity, IAwake<P1, P2>
        {
            var type = typeof(T);
            var component = AddComponent(type);
            (component as IAwake<P1, P2>)?.Awake(p1, p2);
            return component as T;
        }

        public void RemoveComponent(XEntity component)
        {
            if (this.IsDisposed)
                return;
            if (this.m_components == null)
                return;
            var componentType = component.GetType();
            var c = this.GetComponent(componentType);
            if (c == null || c.InstanceId != component.InstanceId)
                return;
            this.RemoveFromComponents(componentType);
            c.Dispose();
        }

        public bool HasComponent(Type type)
        {
            return this.m_components != null && this.m_components.ContainsKey(type);
        }

        private XEntity AddComponentInternal(Type type)
        {
            var component = CreateEntity(type);
            component.Id = this.Id;
            component.Parent = this;
            component.IsComponent = true;
            component.IsActived = this.IsActived;
            this.AddToComponents(type, component);
            return component;
        }

        private void AddToComponents(Type type, XEntity component)
        {
            if (this.m_components == null)
                this.m_components = ObjectPool.Fetch<Dictionary<Type, XEntity>>();
            this.m_components.Add(type, component);
        }

        private void RemoveFromComponents(Type type)
        {
            if (this.m_components == null || !this.m_components.ContainsKey(type))
                return;
            this.m_components.Remove(type);
            if (this.m_components.Count == 0)
            {
                ObjectPool.Recycle(this.m_components);
                this.m_components = null;
            }
        }

        #endregion

        #region Children

        public XEntity AddChild(XEntity xEntity)
        {
            xEntity.Parent = this;
            return xEntity;
        }

        public XEntity AddChild(Type type)
        {
            var child = AddChildInternal(type);
            (child as IAwake)?.Awake();
            return child;
        }

        public T AddChild<T>() where T : XEntity
        {
            var type = typeof(T);
            var child = AddChildInternal(type);
            (child as IAwake)?.Awake();
            return child as T;
        }

        public T AddChild<T, P1>(P1 p1) where T : XEntity, IAwake<P1>
        {
            var type = typeof(T);
            var child = AddChildInternal(type);
            (child as IAwake<P1>)?.Awake(p1);
            return child as T;
        }

        public T AddChild<T, P1, P2>(P1 p1, P2 p2) where T : XEntity, IAwake<P1, P2>
        {
            var type = typeof(T);
            var child = AddChildInternal(type);
            (child as IAwake<P1, P2>)?.Awake(p1, p2);
            return child as T;
        }

        private XEntity AddChildInternal(Type type)
        {
            var child = CreateEntity(type);
            child.Id = IdGenerater.GenerateId();
            child.SetParent(this);
            return child;
        }

        private void AddToChildren(XEntity entity)
        {
            if (this.m_children == null)
                this.m_children = ObjectPool.Fetch<Dictionary<long, XEntity>>();
            this.m_children.Add(entity.Id, entity);
        }

        private void RemoveFromChildren(XEntity entity)
        {
            if (this.m_children == null)
                return;
            this.m_children.Remove(entity.Id);
            if (this.m_children.Count == 0)
            {
                ObjectPool.Recycle(this.m_children);
                this.m_children = null;
            }
        }

        #endregion
    }

    //无具体实现的Entity
    public class XPureEntity : XEntity
    {
    }
}