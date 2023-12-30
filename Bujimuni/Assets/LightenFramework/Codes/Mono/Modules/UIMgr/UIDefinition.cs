using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    //UI排序层级
    public enum EUISortLayer
    {
        Normal = 0, //普通UI层
        Popup, //弹窗UI层
        Overlay, //顶层UI层
        Debug, //调试UI层
    }
    
    //界面可见性剔除模式
    public enum EUICullMode
    {
        None = 0, //不剔除
        All, //剔除所有
        OnlySameLayer, //只剔除同层
    }
    
    //Widget初始状态
    public enum EUIInitState
    {
        AutoHide = 0,//自动隐藏
        AutoShow,//自动显示
        Regardless,//无所谓,不做处理
    }
    
    public enum EUIEntityType
    {
        None = 0,
        Window, //窗口
        Widget, //部件
    }
    
    public static class UIPrefix
    {
        public const string Window = "Dlg";
        public const string Widget = "Wgt";
        public const string OSA = "OSA";
    }
    
    public enum EFilterType
    {
        None = 0,
        OnlyChildren,
        SelfNotChildren,
        SelfAndChildren,
    }
    
    public struct ShowWindowOption
    {
        public static ShowWindowOption Empty { get; private set; } = new ShowWindowOption();
        
        //可见性剔除模式
        public NullableValue<EUICullMode> CullMode { get; set; }
        //是否忽略可见性剔除
        public NullableValue<bool> IsIgnoreCull { get; set; }   
        //是否显示遮罩
        public NullableValue<bool> IsShowMask{ get; set; } 
        //Mask遮罩
        public string MaskName { get; set; }
        //Mask遮罩的Alpha值
        public NullableValue<float> MaskAlpha { get; set; }
        //是否点击空白处关闭
        public NullableValue<bool> IsCloseByClickBlank { get; set; }
    }
}
