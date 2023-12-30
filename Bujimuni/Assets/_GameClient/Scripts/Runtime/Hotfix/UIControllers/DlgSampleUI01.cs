using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public partial class DlgSampleUI01 : UIWindow<DlgSampleUI01.CPara>
{
    public class CPara : IUIShowPara
    {
        //这里写自定义参数
    }

    public override IUIShowPara CreateDefaultPara(string defaultParaText)
    {
        //这里写默认参数的解析
        //var para = ObjectPool.Fetch<Para>();
        return null;
    }

    //注册监听
    protected override void RegisterEvent()
    {
        this.View.OpenButton.AddListener(() =>
        {
            this.CurrentUI().OpenWindowAsync<DlgSampleUI01>().Forget();
        });
        this.View.OpenInstanceButton.AddListener(() =>
        {
            this.CurrentUI().OpenWindowInstanceAsync<DlgSampleUI01>().Forget();
        });
        this.View.CloseButton.AddListener(this.Close);
    }

    //刷新UI
    protected override void RefreshUI()
    {
        int value = Random.Range(0, 100);
        this.View.TitleTextMeshProUGUI.text = $"No:{value}";

        if (value > 50)
        {
            var para = ObjectPool.Fetch<WgtSample01.CPara>();
            para.Name = value.ToString("d3");
            this.View.WgtW3.ForceShow(para);
        }
    }
}