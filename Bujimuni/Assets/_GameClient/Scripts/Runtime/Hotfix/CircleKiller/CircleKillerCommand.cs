using Lighten;

namespace CircleKiller
{
    public class GainScoreCommand : AbstractCommand
    {
        public int Score;
        protected override void OnExecute()
        {
            var model = this.GetModel<CircleKillerModel>();
            model.Score.Value += this.Score;
            if (model.Score >= 20)
            {
                var cmd = this.CreateCommand<GameFinishCommand>();
                cmd.IsWin = true;
                this.SendCommand(cmd);
            }
        }
    }

    public class GameFinishCommand : AbstractCommand
    {
        public bool IsWin;
        protected override void OnExecute()
        {
            this.PublishEvent(new GameFinishEvent() { IsWin = this.IsWin });
            this.GetModel<CircleKillerModel>().IsRunning = false;
        }
    }
}