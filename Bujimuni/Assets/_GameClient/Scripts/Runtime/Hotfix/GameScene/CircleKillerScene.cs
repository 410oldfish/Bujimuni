using System.Threading;
using CircleKiller;
using Cysharp.Threading.Tasks;
using Lighten;

public class CircleKillerScene : GameScene
{
    public override string SceneAssetName { get; set; } = "CircleKiller";
    
    protected override async UniTask OnInit(CancellationToken cancellationToken = default)
    {
        this.GetModel<CircleKillerModel>().Score.Value = 0;
        this.GetModel<CircleKillerModel>().IsRunning = true;
        await this.CurrentUI().OpenWindowAsync<DlgSampleOfCircleKiller>();
        await UniTask.CompletedTask;
    }

    protected override async UniTask OnExit(CancellationToken cancellationToken = default)
    {
        await UniTask.CompletedTask;
    }
}
