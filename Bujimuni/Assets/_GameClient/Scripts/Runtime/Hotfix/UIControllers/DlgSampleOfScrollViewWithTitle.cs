using Lighten;

public partial class DlgSampleOfScrollViewWithTitle : UIWindow<DlgSampleOfScrollViewWithTitle.CPara>
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
        this.View.OSAMultiGroupGrid.SetOnUpdate((vh, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewItem).View.LabelTextMeshProUGUI.text = $"Item:{vh.IndexInGroup}";
        });
        this.View.OSAMultiGroupGrid.SetOnUpdateHeader((groupId, uiEntity) =>
        {
            (uiEntity as WgtSampleOfScrollViewTitle).View.LabelTextMeshProUGUI.text = $"Title:{groupId}";
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        this.View.OSAMultiGroupGrid.ClearGroups();
        this.View.OSAMultiGroupGrid.AddGroup(1, 20);
        this.View.OSAMultiGroupGrid.AddGroup(2, 30);
        this.View.OSAMultiGroupGrid.AddGroup(3, 40);
        this.View.OSAMultiGroupGrid.Show();
    }
}