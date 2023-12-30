using System.Threading;
using Cysharp.Threading.Tasks;
using Lighten;

public class MainScene : GameScene
{
    public override string SceneAssetName { get; set; } = "EmptyScene";

    protected override async UniTask OnInit(CancellationToken cancellationToken = default)
    {
        this.UIComponent.OpenWindowAsync<DlgGameMain>().Forget();
    }

    protected override async UniTask OnExit(CancellationToken cancellationToken = default)
    {
        await UniTask.CompletedTask;
    }
}
