using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    public abstract class UIWindow : UIWidget
    {
        public string WindowName { get; private set; }
        public UIConfig.WindowInfo WindowInfo { get; private set; }
        public Canvas Canvas { get; private set; }
        public EUISortLayer SortLayer { get; private set; }
        public int SortValue { get; private set; }
        public int SortRange { get; private set; }

        //显示参数
        public ShowWindowOption Option { get; set; }

        //是否显示遮罩
        public bool IsShowMask => Option.IsShowMask;

        //遮罩名称
        public string MaskName => Option.MaskName;

        //遮罩透明度
        public float MaskAlpha => this.Option.MaskAlpha;

        //点击空白处关闭
        public bool IsCloseByClickBlank => this.Option.IsCloseByClickBlank;

        //打开界面的时间戳
        public float OpenTime { get; private set; }

        //层级设置器列表
        public List<UISortingLayerSetter> UISortingLayerSetters { get; private set; } =
            new List<UISortingLayerSetter>();

        public void InitWindow(string windowName, UIConfig.WindowInfo windowInfo)
        {
            this.WindowName = windowName;
            this.WindowInfo = windowInfo;
        }

        public void SetShowWindowOption(ShowWindowOption option = default)
        {
            var windowInfo = this.WindowInfo;
            this.SortValue = (int)windowInfo.SortLayer * 1000 + windowInfo.Depth;
            this.SortLayer = windowInfo.SortLayer;
            this.Option = option;

            var obj = this.GameObject;
            this.Canvas = obj.GetComponent<Canvas>();
            this.Canvas.overrideSorting = true;
            //self.Canvas.sortingLayerName = TUIRoot.Instance.UICanvas.sortingLayerName;
            this.Canvas.sortingLayerID = UIRoot.Instance.UICanvas.sortingLayerID;
            this.UISortingLayerSetters.Clear();
            var uiSortingLayerSetters = obj.GetComponentsInChildren<UISortingLayerSetter>();
            foreach (var uiSortingLayerSetter in uiSortingLayerSetters)
            {
                this.UISortingLayerSetters.Add(uiSortingLayerSetter);
                this.SortRange = Mathf.Max(this.SortRange, uiSortingLayerSetter.localSortingOrder);
            }
        }

        public void SetSortingOrder(int value)
        {
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingOrder = value;
            foreach (var uiSortingLayerSetter in this.UISortingLayerSetters)
            {
                uiSortingLayerSetter.UpdateSortingOrder(this.Canvas.sortingLayerID, this.Canvas.sortingOrder);
            }
        }

        public void Close()
        {
            this.GetManager<IUIMgr>().CloseWindowInstance(this.InstanceId);
        }

        protected override void OnShow()
        {
            base.OnShow();
            this.OpenTime = Time.realtimeSinceStartup;
        }
    }

    public abstract class UIWindow<TShowPara> : UIWindow where TShowPara : class, IUIShowPara
    {
        public TShowPara Para => this.m_showPara as TShowPara;
    }
}