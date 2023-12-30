using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    //可监听属性管理器
    public interface IDisposableList
    {
        List<IDisposable> Disposables { get; }
    }

    //扩展Disposeable函数
    public static class DisposableExtentions
    {
        public static void AddTo(this IDisposable disposable, IDisposableList disposableList)
        {
            disposableList.Disposables.Add(disposable);
        }

        public static T AddTo<T>(this T disposable, GameObject gameObject) where T : IDisposable
        {
            if (gameObject == null)
            {
                disposable.Dispose();
                return disposable;
            }
            gameObject.DisposeOnDestroy().Add(disposable);
            return disposable;
        }
        
        public static T AddTo<T>(this T disposable, Component gameObjectComponent) where T : IDisposable
        {
            if (gameObjectComponent == null)
            {
                disposable.Dispose();
                return disposable;
            }

            return AddTo(disposable, gameObjectComponent.gameObject);
        }
        
        public static void DisposeAll(this IDisposableList disposableList)
        {
            if (disposableList.Disposables.Count > 0)
            {
                foreach (var item in disposableList.Disposables)
                {
                    item.Dispose();
                }
                disposableList.Disposables.Clear();
            }
        }
        
        public static DisposeOnDestroy DisposeOnDestroy(this GameObject gameObject)
        {
            var disposeOnDestroy = gameObject.GetComponent<DisposeOnDestroy>();
            if (disposeOnDestroy == null)
            {
                disposeOnDestroy = gameObject.AddComponent<DisposeOnDestroy>();
            }
            return disposeOnDestroy;
        }
    }
}