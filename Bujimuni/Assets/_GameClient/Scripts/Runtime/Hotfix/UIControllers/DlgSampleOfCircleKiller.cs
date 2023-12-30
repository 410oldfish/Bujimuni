using Cysharp.Threading.Tasks;
using Lighten;
using CircleKiller;

public partial class DlgSampleOfCircleKiller : UIWindow<DlgSampleOfCircleKiller.CPara>
{
    public class CPara : IUIShowPara
    {
        //这里写自定义参数
    }

    public override IUIShowPara CreateDefaultPara(string defaultParaText)
    {
        //这里写默认参数的解析
        //var para = ObjectPool.Fetch<CPara>();
        return null;
    }

    //注册监听
    protected override void RegisterEvent()
    {
        this.View.BackButton.AddListener(() => { this.GetManager<IGameSceneMgr>().ChangeScene<MainScene>().Forget(); });

        this.GetModel<CircleKillerModel>().Score.Subscribe(v => { this.View.ScoreTextMeshProUGUI.text = $"Scrore:{v}"; })
            .AddTo(this);

        this.AddListener<GameFinishEvent>(evt =>
        {
            if (evt.IsWin)
            {
                this.CurrentUI().ShowMessage("你赢啦~~~", () =>
                {
                    this.GetManager<IGameSceneMgr>().ChangeScene<MainScene>().Forget();
                });
            }
            else
            {
                this.CurrentUI().ShowMessage("你输啦!!!", () =>
                {
                    this.GetManager<IGameSceneMgr>().ChangeScene<MainScene>().Forget();
                });
            }
            
        }).AddTo(this);
    }

    //刷新UI
    protected override void RefreshUI()
    {
    }
}