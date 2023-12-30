using Cysharp.Threading.Tasks;
using Lighten;

public partial class DlgGameMain : UIWindow<DlgGameMain.CPara>
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
        this.View.BackButton.AddListener(() =>
        {
            this.GetManager<IGameSceneMgr>().ChangeScene<MenuScene>();
        });
        
        this.View.ScrollViewButton.AddListener(() =>
        {
            this.CurrentUI().OpenWindowAsync<DlgSampleOfScrollView>().Forget();
        });
        
        this.View.ScrollViewWithTitleButton.AddListener(() =>
        {
            this.CurrentUI().OpenWindowAsync<DlgSampleOfScrollViewWithTitle>().Forget();
        });
        
        this.View.LoadSpriteButton.AddListener(() =>
        {
            this.CurrentUI().OpenWindowAsync<DlgSampleOfLoadSprite>().Forget();
        });
        
        this.View.CircleKillerButton.AddListener(() =>
        {
            this.GetManager<IGameSceneMgr>().ChangeScene<CircleKillerScene>().Forget();
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        
    }
}