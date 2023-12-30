using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public class UIConfig: ScriptableObject
    {
        [System.Serializable]
        public class WindowLayerInfo
        {
            public EUISortLayer Layer;
            public EUICullMode CullMode;
        }
        public List<WindowLayerInfo> WindowLayerList = new List<WindowLayerInfo>();

        [System.Serializable]
        public class WindowInfo
        {
            public string WindowName;
            public string PrefabName; //预制体名称
            public EUISortLayer SortLayer; //排序层级
            public int Depth; //排序数值
            public bool IsShowMask;//是否显示遮罩
            public string MaskName; //遮罩名称
            public bool IsCloseByClickBlank; //是否点击空白处关闭
            
            public void Clone(WindowInfo source)
            {
                this.SortLayer = source.SortLayer;
                this.Depth = source.Depth;
                this.IsShowMask = source.IsShowMask;
                this.MaskName = source.MaskName;
                this.IsCloseByClickBlank = source.IsCloseByClickBlank;
            }
        }
        public List<WindowInfo> WindowInfos = new List<WindowInfo>();

        [System.Serializable]
        public class WidgetInfo
        {
            public string WidgetName;
            public string PrefabName; //预制体名称
        }
        public List<WidgetInfo> WidgetInfos = new List<WidgetInfo>();
    }
}