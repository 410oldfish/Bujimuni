using Lighten;

public partial class WgtSampleOfScrollViewTitle : UIWidget<WgtSampleOfScrollViewTitle.CPara>
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
        
    }

    //刷新UI
    protected override void RefreshUI()
    {
        
    }
}