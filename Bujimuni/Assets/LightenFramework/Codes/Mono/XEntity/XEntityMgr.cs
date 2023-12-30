using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    //实体根管理器
    [MonoSingletonPath("LightenFramework/XEntityMgr")]
    public class XEntityMgr : MonoSingleton<XEntityMgr>
    {
        //所有实体
        private readonly Dictionary<long, XEntity> m_entities = new Dictionary<long, XEntity>();

        //更新队列
        private readonly Queue<long> m_updates = new Queue<long>();
        private readonly Queue<long> m_lateUpdates = new Queue<long>();
        private readonly Queue<long> m_fixedUpdates = new Queue<long>();
        private readonly Queue<long> m_updatesPerSecond = new Queue<long>();
        
        private float m_timeCounter = 0f;

        public XEntity Root { get; private set; }
        
        private void Awake()
        {
            this.Root = XEntity.CreateRoot();
        }

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();
            if (this.Root != null)
            {
                this.Root.Dispose();
                this.Root = null;
            }
        }
        
        private void Update()
        {
            var queue = this.m_updates;
            int count = queue.Count;
            while (count-- > 0)
            {
                var instanceId = queue.Dequeue();
                var entity = Get(instanceId);
                if (entity == null || entity.IsDisposed)
                    continue;
                queue.Enqueue(instanceId);
                (entity as IUpdate)?.Update(Time.deltaTime);
            }

            m_timeCounter += Time.deltaTime;
            if (m_timeCounter >= 1f)
            {
                this.UpdatePerSecond();
                m_timeCounter = 0f;
            }
        }

        private void UpdatePerSecond()
        {
            var queue = this.m_updatesPerSecond;
            int count = queue.Count;
            while (count-- > 0)
            {
                var instanceId = queue.Dequeue();
                var entity = Get(instanceId);
                if (entity == null || entity.IsDisposed)
                    continue;
                queue.Enqueue(instanceId);
                (entity as IUpdatePerSecond)?.UpdatePerSecond();
            }
        }

        private void LateUpdate()
        {
            var queue = this.m_lateUpdates;
            int count = queue.Count;
            while (count-- > 0)
            {
                var instanceId = queue.Dequeue();
                var entity = Get(instanceId);
                if (entity == null || entity.IsDisposed)
                    continue;
                queue.Enqueue(instanceId);
                (entity as ILateUpdate)?.LateUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            var queue = this.m_fixedUpdates;
            int count = queue.Count;
            while (count-- > 0)
            {
                var instanceId = queue.Dequeue();
                var entity = Get(instanceId);
                if (entity == null || entity.IsDisposed)
                    continue;
                queue.Enqueue(instanceId);
                (entity as IFixedUpdate)?.FixedUpdate(Time.fixedDeltaTime);
            }
        }

        public void Add(XEntity entity)
        {
            var instanceId = entity.InstanceId;
            if (this.m_entities.ContainsKey(instanceId))
            {
                throw new ArgumentException($"Entity already exists. {instanceId}");
            }
            
            if (entity is IUpdate)
            {
                this.m_updates.Enqueue(instanceId);
            }

            if (entity is ILateUpdate)
            {
                this.m_lateUpdates.Enqueue(instanceId);
            }

            if (entity is IFixedUpdate)
            {
                this.m_fixedUpdates.Enqueue(instanceId);
            }

            if (entity is IUpdatePerSecond)
            {
                this.m_updatesPerSecond.Enqueue(instanceId);
            }

            this.m_entities.Add(instanceId, entity);
        }

        public void Remove(long instanceId)
        {
            this.m_entities.Remove(instanceId);
        }

        public XEntity Get(long instanceId)
        {
            XEntity entity = null;
            this.m_entities.TryGetValue(instanceId, out entity);
            return entity;
        }
    }
}