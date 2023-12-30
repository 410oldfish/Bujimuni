
using Lighten;

public class GTaskTrigger_MainGoal : GameTaskTrigger
{
    private IGameTask m_curTask;
    private IGameEventService m_eventService;
    
    protected override void OnEnter()
    {
        m_curTask = GTaskFactory.CreateTask(1);
        m_gameTaskManager.AddTask(m_curTask);
        m_eventService = LightenEntry.GameEventManager.AcquireService();
        m_eventService.AddListener<GEvent_GameTaskComplete>(OnGameTaskComplete);
        m_eventService.AddListener<GEvent_GameTaskGainReward>(OnGameTaskGainReward);
    }

    private void OnGameTaskComplete(IGameEvent obj)
    {
        var param = obj as GEvent_GameTaskComplete;
        if (m_curTask.taskId != param.taskId)
            return;
        m_curTask.GainReward();
    }

    private void OnGameTaskGainReward(IGameEvent obj)
    {
        var param = obj as GEvent_GameTaskGainReward;
        if (m_curTask.taskId != param.taskId)
            return;
        m_curTask = GTaskFactory.CreateTask(1);
        m_gameTaskManager.AddTask(m_curTask);
    }

    protected override void OnExit()
    {
        if (m_curTask != null)
        {
            m_gameTaskManager.RemoveTask(m_curTask);
            m_curTask = null;
        }
        m_eventService.RemoveAll();
    }

    protected override void OnUpdate(float deltaTime)
    {
        
    }
}
