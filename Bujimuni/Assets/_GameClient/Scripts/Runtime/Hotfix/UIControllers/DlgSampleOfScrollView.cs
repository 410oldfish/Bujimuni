using Lighten;

public partial class DlgSampleOfScrollView : UIWindow<DlgSampleOfScrollView.CPara>
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
        this.View.OSAHorizontalList.SetOnUpdate((vh, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewItem).View.LabelTextMeshProUGUI.text = $"Item:{vh.ItemIndex}";
        });
        this.View.OSAHorizontalGrid.SetOnUpdate((vh, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewItem).View.LabelTextMeshProUGUI.text = $"Item:{vh.ItemIndex}";
        });
        this.View.OSAVerticalList.SetOnUpdate((vh, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewItem).View.LabelTextMeshProUGUI.text = $"Item:{vh.ItemIndex}";
        });
        this.View.OSAVerticalGrid.SetOnUpdate((vh, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewItem).View.LabelTextMeshProUGUI.text = $"Item:{vh.ItemIndex}";
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        this.View.OSAHorizontalList.Show(100);
        this.View.OSAHorizontalGrid.Show(100);
        this.View.OSAVerticalList.Show(100);
        this.View.OSAVerticalGrid.Show(100);
    }
}