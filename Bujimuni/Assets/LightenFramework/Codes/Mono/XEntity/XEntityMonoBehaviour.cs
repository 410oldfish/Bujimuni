using UnityEngine;

namespace Lighten
{
    public abstract class XEntityMonoBehaviour<T> : MonoBehaviour where T : XEntity
    {
        public T Entity { get; private set; }

        private bool m_isDestoryed;
        private void OnEnable()
        {
            //Debug.Log($"{gameObject.name}对象被启用");
            this.Entity = Game.Root.AddChild<T>();
            this.Entity.OnDestroy += this.DestoryCallback;
            this.OnEntityAwake();
            this.m_isDestoryed = false;
        }

        private void OnDisable()
        {
            if (this.Entity == null)
                return;
            //Debug.Log($"{gameObject.name}对象被禁用");
            this.OnEntityDestroy();
            this.Entity.OnDestroy -= this.DestoryCallback;
            this.Entity.Dispose();
            this.Entity = null;
        }
        
        //Entity被销毁时，销毁这个GameObject
        private void DestoryCallback()
        {
            //Debug.Log("Entity对象被销毁,同时销毁这个GameObject");
            this.Entity = null;
            Destroy(gameObject);
        }

        protected virtual void OnEntityAwake()
        {
        }

        protected virtual void OnEntityDestroy()
        {
        }
    }
}