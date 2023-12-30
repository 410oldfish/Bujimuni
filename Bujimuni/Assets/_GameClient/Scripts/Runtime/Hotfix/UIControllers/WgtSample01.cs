using Lighten;

public partial class WgtSample01 : UIWidget<WgtSample01.CPara>
{
    public class CPara : IUIShowPara
    {
        //这里写自定义参数
        public string Name { get; set; }
    }
    public override IUIShowPara CreateDefaultPara(string defaultParaText)
    {
        //这里写默认参数的解析
        var para = ObjectPool.Fetch<CPara>();
        para.Name = defaultParaText;
        return para;
    }
    
    //注册监听
    protected override void RegisterEvent()
    {
        
    }

    //刷新UI
    protected override void RefreshUI()
    {
        this.View.NameTextMeshProUGUI.text = this.Para.Name;
    }
}