
using System.Collections.Generic;

namespace Lighten
{
    public abstract class GameTask : IGameTask
    {
        protected EGameTaskState m_state;
        protected int m_taskId;
        protected int m_taskProgress;
        protected bool m_triggerComplete;
        
        public EGameTaskState state => m_state;
        public int taskId => m_taskId;
        public int taskProgress => m_taskProgress;

        public void SetTaskId(int taskId)
        {
            m_taskId = taskId;
        }

        public void Update(float deltaTime)
        {
            switch (m_state)
            {
                case EGameTaskState.None:
                    OnEnter();
                    m_state = EGameTaskState.Running;
                    m_triggerComplete = false;
                    GEvent_GameTaskStart.Trigger(taskId);
                    Utility.Debug.Log(LightenConst.TAG, $"任务开始 {taskId}");
                    break;
                case EGameTaskState.Running:
                    OnUpdate(deltaTime);
                    if (m_triggerComplete)
                    {
                        OnExit();
                        m_state = EGameTaskState.Complete;
                        GEvent_GameTaskComplete.Trigger(taskId);
                        Utility.Debug.Log(LightenConst.TAG, $"任务完成 {taskId}");
                    }
                    break;
                case EGameTaskState.Complete:
                    //任务完成,等待领取奖励
                    break;
                case EGameTaskState.Rewarded:
                    //已领取奖励
                    GEvent_GameTaskGainReward.Trigger(taskId);
                    m_state = EGameTaskState.Destroy;
                    break;
                case EGameTaskState.Destroy:
                    //等待销毁
                    break;
            }
        }
        public void GainReward()
        {
            m_state = EGameTaskState.Rewarded;
        }
        protected void TaskComplete()
        {
            m_triggerComplete = true;
        }
        protected void SetTaskProgress(int progress)
        {
            m_taskProgress = progress;
            GEvent_GameTaskProgressChange.Trigger(taskId);
            Utility.Debug.Log(LightenConst.TAG, $"任务进度 {taskId}:{m_taskProgress}");
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnUpdate(float deltaTime);
    }
}
