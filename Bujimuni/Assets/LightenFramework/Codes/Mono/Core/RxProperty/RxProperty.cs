using System;
using System.Collections.Generic;

namespace Lighten
{
    public interface IReadonlyRxProperty<T>
    {
        T Value { get; }
        IUnRegisterable SubscribeWithoutFirstNotify(Action<T> action);
    }

    public interface IRxProperty<T> : IReadonlyRxProperty<T>
    {
        new T Value { get; set; }
        void SetValueWithoutNotify(T newValue);
        void SetValueAndForceNotify(T value);
    }

    //可监听属性
    public class RxProperty<T> : RxObservable<T>, IRxProperty<T>
    {
        private T m_value;
        
        public T Value
        {
            get => this.m_value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(this.m_value, value))
                    return;
                SetValue(value);
                Notify(value);
            }
        }

        public RxProperty() : this(default(T))
        {
        }

        public RxProperty(T initialValue)
        {
            SetValue(initialValue);
        }

        public void SetValueWithoutNotify(T value)
        {
            SetValue(value);
        }

        public void SetValueAndForceNotify(T value)
        {
            SetValue(value);
            Notify(value);
        }

        public override IUnRegisterable Subscribe(Action<T> action)
        {
            action?.Invoke(this.Value);
            return base.Subscribe(action);
        }

        public IUnRegisterable SubscribeWithoutFirstNotify(Action<T> action)
        {
            return base.Subscribe(action);
        }
        
        public static implicit operator T(RxProperty<T> property)
        {
            return property.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
        
        protected virtual void SetValue(T value)
        {
            this.m_value = value;
        }
    }
}