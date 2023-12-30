using Lighten;

public partial class DlgGameMenu : UIWindow<DlgGameMenu.CPara>
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
        this.View.EnterGameButton.AddListener(() =>
        {
            this.GetManager<IGameSceneMgr>().ChangeScene<MainScene>();
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        
    }
}