using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public readonly struct XEntityRef<T> where T : XEntity
    {
        private readonly long m_instanceId;
        private readonly T m_entity;

        private XEntityRef(T entity)
        {
            this.m_instanceId = entity.InstanceId;
            this.m_entity = entity;
        }

        private T UnWrap
        {
            get
            {
                if (this.m_entity == null)
                    return null;
                if (this.m_entity.InstanceId != this.m_instanceId)
                    return null;
                return this.m_entity;
            }
        }
        
        public static implicit operator XEntityRef<T>(T t)
        {
            return new XEntityRef<T>(t);
        }

        public static implicit operator T(XEntityRef<T> v)
        {
            return v.UnWrap;
        }
    }
}
