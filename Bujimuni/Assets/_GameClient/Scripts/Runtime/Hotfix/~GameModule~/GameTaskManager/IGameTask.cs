
using System.Collections.Generic;

namespace Lighten
{
    public enum EGameTaskState
    {
        None = 0,
        Running = 1,
        Complete = 2,
        Rewarded = 3,
        Destroy = 4,
    }
    public interface IGameTask
    {
        int taskId { get; }
        int taskProgress { get; }
        EGameTaskState state { get; }
        void SetTaskId(int taskId);
        void Update(float deltaTime);
        void GainReward();
    }
}
