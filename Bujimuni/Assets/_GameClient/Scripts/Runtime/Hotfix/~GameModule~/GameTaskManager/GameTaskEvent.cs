//
// namespace Lighten
// {
//     //任务开始事件
//     public class GEvent_GameTaskStart : GameEvent<GEvent_GameTaskStart>
//     {
//         public int taskId;
//         
//         public static void Trigger(int taskId)
//         {
//             var evt = LightenEntry.GameEventManager.AcquireEvent<GEvent_GameTaskStart>();
//             evt.taskId = taskId;
//             evt.PostEvent();     
//         }
//     }
//     
//     //任务进度变化事件
//     public class GEvent_GameTaskProgressChange : GameEvent<GEvent_GameTaskProgressChange>
//     {
//         public int taskId;
//         
//         public static void Trigger(int taskId)
//         {
//             var evt = LightenEntry.GameEventManager.AcquireEvent<GEvent_GameTaskProgressChange>();
//             evt.taskId = taskId;
//             evt.PostEvent();     
//         }
//     }
//     
//     //任务完成事件
//     public class GEvent_GameTaskComplete : GameEvent<GEvent_GameTaskComplete>
//     {
//         public int taskId;
//         
//         public static void Trigger(int taskId)
//         {
//             var evt = LightenEntry.GameEventManager.AcquireEvent<GEvent_GameTaskComplete>();
//             evt.taskId = taskId;
//             evt.PostEvent();     
//         }
//     }
//     
//     //任务奖励领取事件
//     public class GEvent_GameTaskGainReward : GameEvent<GEvent_GameTaskGainReward>
//     {
//         public int taskId;
//         
//         public static void Trigger(int task)
//         {
//             var evt = LightenEntry.GameEventManager.AcquireEvent<GEvent_GameTaskGainReward>();
//             evt.taskId = task;
//             evt.PostEvent();     
//         }
//     }
// }
