using Lighten;

public partial class DlgSampleOfLoadSprite : UIWindow<DlgSampleOfLoadSprite.CPara>
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
        this.View.LoadButton.AddListener(() =>
        {
            var inputName = this.View.InputNameTMP_InputField.text.Trim();
            if (string.IsNullOrEmpty(inputName))
                return;
            Utility.Debug.Log("GGYY", $"加载图片 {inputName}");
            this.SetSprite(this.View.ImgImage, inputName);
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        
    }
}