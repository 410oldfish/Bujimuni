
using Lighten;

public static class GTaskFactory
{
    public static IGameTask CreateTask(int taskId)
    {
        var task = new GTask_WaitTime();
        //task.SetTaskId(taskId);
        return task;
    }
}
