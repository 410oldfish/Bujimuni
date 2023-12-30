//
// namespace Lighten
// {
//     public abstract class GameTaskTrigger : IGameTaskTrigger
//     {
//         protected IGameTaskManager m_gameTaskManager;
//         protected bool m_running;
//         public void Init(IGameTaskManager gameTaskManager)
//         {
//             m_gameTaskManager = gameTaskManager;
//             m_running = false;
//         }
//
//         public void Start()
//         {
//             if (m_running)
//                 return;
//             OnEnter();
//             m_running = true;
//         }
//
//         public void Stop()
//         {
//             if (!m_running)
//                 return;
//             OnExit();
//             m_running = false;
//         }
//
//         public void Update(float deltaTime)
//         {
//             if (m_running)
//             {
//                 OnUpdate(deltaTime);
//             }
//         }
//
//         protected abstract void OnEnter();
//         protected abstract void OnExit();
//         protected abstract void OnUpdate(float deltaTime);
//     }
// }
