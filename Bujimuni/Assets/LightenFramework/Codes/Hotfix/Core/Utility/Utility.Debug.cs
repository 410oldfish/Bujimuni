using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Lighten
{
    public static partial class Utility
    {
        public static class Debug
        {
#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void Log(string msg)
            {
                UnityEngine.Debug.Log(msg);
            }

#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void Log(string tag, string msg)
            {
                UnityEngine.Debug.Log($"[{tag}] {msg}");
            }

#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void Log(string tag, Func<string> msgMaker)
            {
                UnityEngine.Debug.Log($"[{tag}] {msgMaker()}");
            }

#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void LogWarning(string msg)
            {
                UnityEngine.Debug.LogWarning(msg);
            }

#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void LogError(string msg)
            {
                UnityEngine.Debug.LogError(msg);
            }

            private static Dictionary<string, float> m_dictLastTime = new Dictionary<string, float>();
            
#if !UNITY_EDITOR
            [Conditional("XDEBUG")]
#endif
            public static void LogWithTime(string tag, string msg)
            {
                float realtimeSinceStartup = Time.realtimeSinceStartup;
                var lastTime = 0f;
                m_dictLastTime.TryGetValue(tag, out lastTime);
                UnityEngine.Debug.Log($"[{tag}] [{realtimeSinceStartup - lastTime}s] {msg}");
                m_dictLastTime[tag] = realtimeSinceStartup;
            }
        }
    }
}