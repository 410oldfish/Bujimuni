using UnityEngine;

namespace Lighten
{
    public static class GameObjectExtension
    {
        // public static T GetHotfixMono<T>(this GameObject go) where T : HotfixMono
        // {
        //     var component = go.GetComponent<HotfixMonoLinker>();
        //     if (component == null)
        //         return null;
        //     return component.GetHotfixMono<T>();
        // }

        public static void SafeRemoveComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component == null)
                return;
#if UNITY_EDITOR
            Object.DestroyImmediate(component);
#else
            Object.Destroy(component);
#endif
        }

        public static T SafeAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component != null)
                return component;
            return go.AddComponent<T>();
        }
    }
}