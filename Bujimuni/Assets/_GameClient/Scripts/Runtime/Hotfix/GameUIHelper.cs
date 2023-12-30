
using System;
using Cysharp.Threading.Tasks;
using Lighten;

public static class GameUIHelper
{
    public static void ShowMessage(this UIComponent uiComponent, string message, Action onConfirm = null)
    {
        var para = ObjectPool.Fetch<DlgMessageBox.CPara>();
        para.Message = message;
        para.OnConfirm = onConfirm;
        uiComponent.OpenWindowAsync<DlgMessageBox>(para).Forget();
    }
}
