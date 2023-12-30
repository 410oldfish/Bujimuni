using System;
using Lighten;

public partial class DlgMessageBox : UIWindow<DlgMessageBox.CPara>
{
    public class CPara : IUIShowPara
    {
        //这里写自定义参数
        public string Message;
        public Action OnConfirm;
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
        this.View.ConfirmButton.AddListener(() =>
        {
            this.Para.OnConfirm?.Invoke();
            this.Close();
        });
        this.View.CancelButton.AddListener(() =>
        {
            this.Close();
        });
    }

    //刷新UI
    protected override void RefreshUI()
    {
        this.View.MessageTextMeshProUGUI.text = this.Para.Message;
    }
}