using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public enum EUnRegisterTrigger
    {
        Disable = 0,
        Destroy,
    }

    public interface IUnRegisterable
    {
        void UnRegister();
    }

    public interface IUnRegisterableList
    {
        List<IUnRegisterable> UnRegisterables { get; }
    }

    public static class UnRegisterableExtensions
    {
        public static void AddTo(this IUnRegisterable self, IUnRegisterableList list)
        {
            list.UnRegisterables.Add(self);
        }

        public static T AddTo<T>(this T self, GameObject gameObject,
            EUnRegisterTrigger trigger = EUnRegisterTrigger.Disable) where T : IUnRegisterable
        {
            if (gameObject == null)
            {
                self.UnRegister();
                return self;
            }

            switch (trigger)
            {
                case EUnRegisterTrigger.Disable:
                    gameObject.UnRegisterTriggerOnDisable().Add(self);
                    break;
                case EUnRegisterTrigger.Destroy:
                    gameObject.UnRegisterTriggerOnDestroy().Add(self);
                    break;
                default:
                    throw new Exception($"{typeof(T)} 未处理该类型触发器: {trigger}");
                    break;
            }

            return self;
        }

        public static T AddTo<T>(this T self, Component gameObjectComponent,
            EUnRegisterTrigger trigger = EUnRegisterTrigger.Disable) where T : IUnRegisterable
        {
            return AddTo(self, gameObjectComponent.gameObject, trigger);
        }


        public static void UnRegisterAll(this IUnRegisterableList self)
        {
            if (self.UnRegisterables.Count > 0)
            {
                foreach (var item in self.UnRegisterables)
                {
                    item.UnRegister();
                }

                self.UnRegisterables.Clear();
            }
        }

        public static void UnRegisterAll(this GameObject gameObject)
        {
            gameObject.UnRegisterTriggerOnDisable().UnRegisterAll();
            gameObject.UnRegisterTriggerOnDestroy().UnRegisterAll();
        }

        private static UnRegisterTriggerOnDisable UnRegisterTriggerOnDisable(this GameObject gameObject)
        {
            var mono = gameObject.GetComponent<UnRegisterTriggerOnDisable>();
            if (mono == null)
            {
                mono = gameObject.AddComponent<UnRegisterTriggerOnDisable>();
            }

            return mono;
        }

        private static UnRegisterTriggerOnDestroy UnRegisterTriggerOnDestroy(this GameObject gameObject)
        {
            var mono = gameObject.GetComponent<UnRegisterTriggerOnDestroy>();
            if (mono == null)
            {
                mono = gameObject.AddComponent<UnRegisterTriggerOnDestroy>();
            }

            return mono;
        }
    }
}