using System.Threading;
using Cysharp.Threading.Tasks;
using Lighten;

public class MenuScene : GameScene
{
    protected override async UniTask OnInit(CancellationToken cancellationToken = default)
    {
        await UniTask.CompletedTask;
        this.UIComponent.OpenWindowAsync<DlgGameLogin>().Forget();
    }

    protected override async UniTask OnExit(CancellationToken cancellationToken = default)
    {
        await UniTask.CompletedTask;
    }
}
