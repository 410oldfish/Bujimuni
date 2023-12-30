using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Lighten
{
    public interface ICanPublishEvent
    {
    }

    public interface ICanSubscribeEvent
    {
    }

    public static class CanSendEventExtension
    {
        #region 订阅事件

        public static IUnRegisterable AddListener<T>(this ICanSubscribeEvent self, Action<T> action) where T : struct
        {
            return EventManager.Instance.AddListener(action);
        }

        public static void RemoveListener<T>(this ICanSubscribeEvent self, Action<T> action) where T : struct
        {
            EventManager.Instance.RemoveListener(action);
        }

        public static IUnRegisterable AddListener<T>(this ICanSubscribeEvent self, Func<T, CancellationToken, UniTask> action) where T : struct
        {
            return EventManager.Instance.AddListener(action);
        }

        public static void RemoveListener<T>(this ICanSubscribeEvent self, Func<T, CancellationToken, UniTask> action) where T : struct
        {
            EventManager.Instance.RemoveListener(action);
        }

        #endregion

        #region 发布事件

        public static void PublishEvent<T>(this ICanPublishEvent self) where T : struct
        {
            EventManager.Instance.Publish<T>();
        }

        public static void PublishEvent<T>(this ICanPublishEvent self, T e) where T : struct
        {
            EventManager.Instance.Publish(e);
        }

        public static async UniTask PublishEventAsync<T>(this ICanPublishEvent self, CancellationToken cancellationToken) where T : struct
        {
            await EventManager.Instance.PublishAsync<T>(cancellationToken);
        }

        public static async UniTask PublishEventAsync<T>(this ICanPublishEvent self, T e, CancellationToken cancellationToken) where T : struct
        {
            await EventManager.Instance.PublishAsync(e, cancellationToken);
        }

        #endregion
    }
}