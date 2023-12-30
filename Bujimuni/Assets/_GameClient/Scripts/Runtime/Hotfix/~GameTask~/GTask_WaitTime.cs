
using Lighten;

public class GTask_WaitTime : GameTask
{
    protected float m_time;
    protected float m_timeCounter;
    protected override void OnEnter()
    {
        m_time = 1;
        m_timeCounter = 0f;
    }

    protected override void OnExit()
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        m_timeCounter += deltaTime;
        if (m_timeCounter < m_time)
            return;
        TaskComplete();
    }
}
