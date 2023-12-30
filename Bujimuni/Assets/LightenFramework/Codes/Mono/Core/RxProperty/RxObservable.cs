using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public interface IRxObservable<T> : IDisposable
    {
        IUnRegisterable Subscribe(Action<T> action);
        void CancelSubscribe(Action<T> action);
        void Notify(T value);
    }
    
    //可观察对象
    public class RxObservable<T> : IRxObservable<T>
    {
        private Action<T> m_action = (v) => { };

        public virtual IUnRegisterable Subscribe(Action<T> action)
        {
            m_action += action;
            return this.CreateDisposer(action);
        }

        public void CancelSubscribe(Action<T> action)
        {
            m_action -= action;
        }

        public void Notify(T value)
        {
            m_action.Invoke(value);
        }

        public void Dispose()
        {
            this.m_action = null;
        }
    }

    //注销器
    public class SubscribeDisposer<T> : IUnRegisterable
    {
        private IRxObservable<T> m_observable;
        private Action<T> m_action;

        public void Set(IRxObservable<T> observable, Action<T> action)
        {
            this.m_observable = observable;
            this.m_action = action;
        }

        public void UnRegister()
        {
            this.m_observable.CancelSubscribe(m_action);
            this.m_observable = null;
            this.m_action = null;
        }
    }

    public interface IRxObservableAsync<T> : IDisposable
    {
        IUnRegisterable Subscribe(Func<T, CancellationToken, UniTask> action);
        void CancelSubscribe(Func<T, CancellationToken, UniTask> action);
        UniTask NotifyAsync(T value, CancellationToken cancellationToken);
    }

    public class RxObservableAsync<T> : IRxObservableAsync<T>
    {
        private List<Func<T, CancellationToken, UniTask>> m_actions = new List<Func<T, CancellationToken, UniTask>>();
        private List<Func<T, CancellationToken, UniTask>> m_runtime = new List<Func<T, CancellationToken, UniTask>>();
        private List<UniTask> m_utss = new List<UniTask>();
        
        public virtual IUnRegisterable Subscribe(Func<T, CancellationToken, UniTask> action)
        {
            m_actions.Add(action);
            return this.CreateDisposer(action);
        }

        public void CancelSubscribe(Func<T, CancellationToken, UniTask> action)
        {
            m_actions.Remove(action);
        }

        public async UniTask NotifyAsync(T value, CancellationToken cancellationToken)
        {
            m_runtime.Clear();
            m_runtime.AddRange(this.m_actions);
            m_utss.Clear();
            foreach (var action in m_runtime)
            {
                m_utss.Add(action.Invoke(value, cancellationToken));
            }
            await UniTask.WhenAll(m_utss);
        }

        public void Dispose()
        {
            m_actions.Clear();
        }
    }

    public class SubscribeDisposerAsync<T> : IUnRegisterable
    {
        private IRxObservableAsync<T> m_observable;
        private Func<T, CancellationToken, UniTask> m_action;

        public void Set(IRxObservableAsync<T> observable, Func<T, CancellationToken, UniTask> action)
        {
            this.m_observable = observable;
            this.m_action = action;
        }

        public void UnRegister()
        {
            this.m_observable.CancelSubscribe(m_action);
            this.m_observable = null;
            this.m_action = null;
        }
    }

    //扩展接口
    public static class RxObservableExtension
    {
        public static IUnRegisterable CreateDisposer<T>(this IRxObservable<T> observable, Action<T> action)
        {
            var disposer = ObjectPool.Fetch<SubscribeDisposer<T>>();
            disposer.Set(observable, action);
            return disposer;
        }

        public static IUnRegisterable CreateDisposer<T>(this IRxObservableAsync<T> observable, Func<T, CancellationToken, UniTask> action)
        {
            var disposer = ObjectPool.Fetch<SubscribeDisposerAsync<T>>();
            disposer.Set(observable, action);
            return disposer;
        }
    }
}