// using System;
// using System.Collections.Generic;
//
// namespace Lighten
// {
//     [Module]
//     public sealed class TimerManager : Module, ITimerManager
//     {
//         public struct Timer
//         {
//             public long id;
//             public float time;
//             public Action callback;
//             public int loop;
//
//             public float currentTime;
//             public int currentLoop;
//         }
//
//         private long m_idCount = 0;
//         private List<Timer> m_timers = new List<Timer>();
//         private List<Timer> m_delayAddTimers = new List<Timer>();
//         private List<long> m_delayRemoveTimers = new List<long>();
//
//         public void Update(float deltaTime)
//         {
//             if (m_delayAddTimers.Count > 0)
//             {
//                 foreach (var timer in m_delayAddTimers)
//                 {
//                     AddTimerInternal(timer);
//                 }
//
//                 m_delayAddTimers.Clear();
//             }
//
//             if (m_delayRemoveTimers.Count > 0)
//             {
//                 foreach (var timerId in m_delayRemoveTimers)
//                 {
//                     RemoveTimerInternal(timerId);
//                 }
//
//                 m_delayRemoveTimers.Clear();
//             }
//
//             if (m_timers.Count > 0)
//             {
//                 for (int i = 0; i < m_timers.Count; ++i)
//                 {
//                     var timer = m_timers[i];
//                     timer.currentTime += deltaTime;
//                     if (timer.currentTime >= timer.time)
//                     {
//                         timer.callback?.Invoke();
//                         ++timer.currentLoop;
//                         if (timer.loop > 0 && timer.currentLoop >= timer.loop)
//                         {
//                             RemoveTimer(timer.id);
//                             continue;
//                         }
//                         timer.currentTime = 0;
//                     }
//                     m_timers[i] = timer;
//                 }
//             }
//         }
//
//         public long AddTimer(float time, Action callback, int loop = -1)
//         {
//             var id = GenerateId();
//             var timer = CreateTimer(id, time, callback, loop);
//             m_delayAddTimers.Add(timer);
//             return id;
//         }
//
//         public void RemoveTimer(long id)
//         {
//             if (m_delayRemoveTimers.Contains(id))
//                 return;
//             m_delayRemoveTimers.Add(id);
//         }
//
//         private void AddTimerInternal(Timer timer)
//         {
//             m_timers.Add(timer);
//         }
//
//         private void RemoveTimerInternal(long id)
//         {
//             for (int i = 0; i < m_timers.Count; ++i)
//             {
//                 if (m_timers[i].id == id)
//                 {
//                     m_timers.RemoveAt(i);
//                     break;
//                 }
//             }
//         }
//
//         private long GenerateId()
//         {
//             return ++m_idCount;
//         }
//
//         private Timer CreateTimer(long id, float time, Action callback, int loop)
//         {
//             var timer = new Timer();
//             timer.id = id;
//             timer.time = time;
//             timer.callback = callback;
//             timer.loop = loop;
//             return timer;
//         }
//     }
// }