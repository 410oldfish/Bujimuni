// using System.Collections.Generic;
//
// namespace Lighten
// {
//     [Module(100)]
//     public sealed class GameTaskManager : Module, IGameTaskManager
//     {
//         private List<IGameTaskTrigger> m_listTaskTriggers = new List<IGameTaskTrigger>();
//         private Dictionary<string, IGameTaskTrigger> m_dictTaskTriggers = new Dictionary<string, IGameTaskTrigger>();
//
//         private List<IGameTask> m_appendTasks = new List<IGameTask>();
//         private List<IGameTask> m_removeTasks = new List<IGameTask>();
//         private List<IGameTask> m_listTasks = new List<IGameTask>();
//         private Dictionary<int, IGameTask> m_dictTasks = new Dictionary<int, IGameTask>();
//
//         public void Update(float deltaTime)
//         {
//             if (m_listTaskTriggers.Count > 0)
//             {
//                 foreach (var taskTrigger in m_listTaskTriggers)
//                 {
//                     taskTrigger.Update(deltaTime);
//                 }
//             }
//             if (m_appendTasks.Count > 0)
//             {
//                 foreach (var task in m_appendTasks)
//                 {
//                     AddTaskInternal(task);
//                 }
//                 m_appendTasks.Clear();
//             }
//             if (m_listTasks.Count > 0)
//             {
//                 foreach (var task in m_listTasks)
//                 {
//                     task.Update(deltaTime);
//                     if (task.state == EGameTaskState.Destroy)
//                     {
//                         m_removeTasks.Add(task);
//                     }
//                 }
//             }
//             if (m_removeTasks.Count > 0)
//             {
//                 foreach (var task in m_removeTasks)
//                 {
//                     RemoveTaskInternal(task);
//                 }
//                 m_removeTasks.Clear();
//             }
//         }
//         public IGameTask GetTask(int taskId)
//         {
//             if (m_dictTasks.ContainsKey(taskId))
//                 return m_dictTasks[taskId];
//             return null;
//         }
//
//         public void AddTaskTrigger<T>() where T : class, IGameTaskTrigger, new()
//         {
//             var triggerName = typeof(T).Name;
//             if (m_dictTaskTriggers.ContainsKey(triggerName))
//                 return;
//             var trigger = new T();
//             trigger.Init(this);
//             trigger.Start();
//             m_listTaskTriggers.Add(trigger);
//             m_dictTaskTriggers.Add(triggerName, trigger);
//         }
//
//         public void RemoveTaskTrigger<T>() where T : class, IGameTaskTrigger
//         {
//             var triggerName = typeof(T).Name;
//             if (!m_dictTaskTriggers.ContainsKey(triggerName))
//                 return;
//             var trigger = m_dictTaskTriggers[triggerName];
//             trigger.Stop();
//             m_listTaskTriggers.Remove(trigger);
//             m_dictTaskTriggers.Remove(triggerName);
//         }
//
//         public void AddTask(IGameTask task)
//         {
//             m_appendTasks.Add(task);
//         }
//         public void RemoveTask(IGameTask task)
//         {
//             m_removeTasks.Add(task);
//         }
//         public void RemoveTask(int taskId)
//         {
//             if (!m_dictTasks.ContainsKey(taskId))
//                 return;
//             m_removeTasks.Add(m_dictTasks[taskId]);
//         }
//
//         private void AddTaskInternal(IGameTask task)
//         {
//             var id = task.taskId;
//             if (m_dictTasks.ContainsKey(id))
//                 return;
//             m_listTasks.Add(task);
//             m_dictTasks.Add(id, task);
//         }
//         private void RemoveTaskInternal(IGameTask task)
//         {
//             var id = task.taskId;
//             if (!m_dictTasks.ContainsKey(id))
//                 return;
//             m_listTasks.Remove(task);
//             m_dictTasks.Remove(id);
//         }
//     }
// }