using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public interface IEventHandler : IDisposable
    {
    }
    
    public class EventHandler<T> : RxObservable<T>, IEventHandler
    {
    }
    
    public class EventHandlerAsync<T> : RxObservableAsync<T>, IEventHandler
    {

    }
    

    [MonoSingletonPath("Lighten/EventManager")]
    public class EventManager : MonoSingleton<EventManager>
    {
        private Dictionary<Type, IEventHandler> m_eventHandlerDict = new Dictionary<Type, IEventHandler>();

        public override void OnSingletonDestroy()
        {
            base.OnSingletonDestroy();
            if (m_eventHandlerDict.Count > 0)
            {
                foreach (var eventHandler in m_eventHandlerDict.Values)
                {
                    eventHandler.Dispose();
                }
                m_eventHandlerDict.Clear();
            }
        }

        public void Publish<T>() where T : struct
        {
            Publish(new T());
        }

        public void Publish<T>(T evt) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandler<T>>();
            handler.Notify(evt);
            var handlerAsync = GetOrCreateEventHandler<EventHandlerAsync<T>>();
            handlerAsync.NotifyAsync(evt, CancellationToken.None).Forget();
        }
        
        public async UniTask PublishAsync<T>(CancellationToken cancellationToken) where T : struct
        {
            await PublishAsync(new T(), cancellationToken);
        }

        public async UniTask PublishAsync<T>(T evt, CancellationToken cancellationToken) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandler<T>>();
            handler.Notify(evt);
            var handlerAsync = GetOrCreateEventHandler<EventHandlerAsync<T>>();
            await handlerAsync.NotifyAsync(evt, cancellationToken);
        }

        public IUnRegisterable AddListener<T>(Action<T> action) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandler<T>>();
            return handler.Subscribe(action);
        }

        public void RemoveListener<T>(Action<T> action) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandler<T>>();
            handler.CancelSubscribe(action);
        }
        
        public IUnRegisterable AddListener<T>(Func<T, CancellationToken, UniTask> action) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandlerAsync<T>>();
            return handler.Subscribe(action);
        }

        public void RemoveListener<T>(Func<T, CancellationToken, UniTask> action) where T : struct
        {
            var handler = GetOrCreateEventHandler<EventHandlerAsync<T>>();
            handler.CancelSubscribe(action);
        }
        
        private T GetOrCreateEventHandler<T>() where T : class, IEventHandler, new()
        {
            var type = typeof(T);
            if (m_eventHandlerDict.TryGetValue(type, out var evt))
            {
                return evt as T;
            }
            var handler = new T();
            m_eventHandlerDict.Add(type, handler);
            return handler;
        }
    }
}