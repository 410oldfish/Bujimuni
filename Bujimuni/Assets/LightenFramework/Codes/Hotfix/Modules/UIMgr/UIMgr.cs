using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Lighten
{
    public interface IUIMgr
    {
        UniTask<long> OpenWindowAsync<TWindow>(IUIShowPara showPara = null, ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow;

        UniTask<long> OpenWindowInstanceAsync<TWindow>(IUIShowPara showPara = null,
            ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow;

        long GetWindowInstanceId<TWindow>() where TWindow : UIWindow;
        void CloseWindowInstance(long instanceId);
    }

    public class UIMgr : AbstractManager, IUIMgr, IAwake, IUpdate, IDestroy
    {
        private ResLoader m_resLoader;
        private UniTaskCTS m_uniTaskCTS;

        private bool m_isConfigLoaded;
        private UIConfig m_uiConfig;
        private Transform m_maskRoot;
        private Canvas m_maskCanvas;
        private GameObject m_defaultMask;
        private GameObject m_currentMask;

        private Canvas m_blankCanvas;
        private Transform m_blankRoot;
        private long m_currentBlankOwnerInstanceId;

        private bool m_needResort;

        //
        private Dictionary<string, Stack<long>> m_visibleWindowByNameDict = new Dictionary<string, Stack<long>>();
        private Dictionary<long, UIWindow> m_visibleWindowDict = new Dictionary<long, UIWindow>();
        private List<UIWindow> m_visibleSortedWindows = new List<UIWindow>();
        private List<UIWindow> m_finallySortedWindows = new List<UIWindow>();


        public void Awake()
        {
            this.m_resLoader = this.AddComponent<ResLoader>();
            this.m_uniTaskCTS = this.AddComponent<UniTaskCTS>();

            this.m_maskCanvas = UIRoot.Instance.UICanvasOfMask;
            this.m_maskRoot = this.m_maskCanvas.transform;
            this.m_defaultMask = this.m_maskRoot.Find("Default").gameObject;

            this.m_blankCanvas = UIRoot.Instance.UICanvasOfBlank;
            this.m_blankRoot = this.m_blankCanvas.transform;
            this.m_blankRoot.GetComponent<Button>().onClick.AddListener(this.OnClickBlank);

            this.m_isConfigLoaded = false;
        }

        public void Destroy()
        {
            this.m_uiConfig = null;
        }

        public void Update(float elapsedTime)
        {
            if (this.m_needResort)
            {
                this.SortWindows();
                this.m_needResort = false;
            }
        }

        private async UniTask CheckConfigLoaded()
        {
            if (this.m_isConfigLoaded)
                return;
            this.m_uiConfig = await this.m_resLoader.LoadAssetAsync<UIConfig>("UIConfig");
            this.m_isConfigLoaded = true;
        }

        #region 事件处理

        private void OnClickBlank()
        {
            if (this.m_currentBlankOwnerInstanceId <= 0)
                return;
            CloseWindowInstance(this.m_currentBlankOwnerInstanceId);
            this.m_currentBlankOwnerInstanceId = -1;
        }

        #endregion

        #region 配置表

        public UIConfig.WindowInfo GetWindowInfo(string windowName)
        {
            return this.m_uiConfig.WindowInfos.Find(d => d.WindowName == windowName);
        }

        public UIConfig.WidgetInfo GetWidgetInfo(string widgetName)
        {
            return this.m_uiConfig.WidgetInfos.Find(d => d.WidgetName == widgetName);
        }

        public UIConfig.WindowLayerInfo GetWindowLayerInfo(EUISortLayer layer)
        {
            return this.m_uiConfig.WindowLayerList.Find(d => d.Layer == layer);
        }

        #endregion

        #region 界面创建

        //创建窗体
        private async UniTask<UIWindow> CreateWindowAsync(string windowName, Type windowType, bool createInstance,
            CancellationToken cancellationToken = default)
        {
            var windowInfo = this.GetWindowInfo(windowName);
            if (windowInfo == null)
                return null;
            using (await this.m_uniTaskCTS.Lock(windowName, cancellationToken))
            {
                UIWindow uiWindow = GetWindowByName(windowName);
                if (uiWindow != null && !createInstance)
                {
                    return uiWindow;
                }

                uiWindow = this.AddChild(windowType) as UIWindow;
                if (uiWindow == null)
                {
                    throw new Exception($"界面创建失败 没有找到该类型 windowType:{windowType}");
                }

                uiWindow.InitWindow(windowName, windowInfo);
                //加载资源
                await uiWindow.LoadAsync(windowInfo.PrefabName, UIRoot.Instance.UICanvas.transform,
                    cancellationToken);
                this.AddWindowToStack(uiWindow.InstanceId, uiWindow);
                return uiWindow;
            }
        }

        //销毁窗体
        private void RemoveWindow(long instanceId)
        {
            var uiWindow = GetWindow(instanceId);
            if (uiWindow == null)
                return;
            uiWindow.Dispose();
            RemoveWindowFromStack(instanceId);
        }

        //创建显示参数
        private ShowWindowOption CombineWindowShowOption(UIConfig.WindowInfo windowInfo,
            ShowWindowOption option = default)
        {
            var defaultCullMode = EUICullMode.None;
            var windowLayerInfo = this.GetWindowLayerInfo(windowInfo.SortLayer);
            if (windowLayerInfo != null)
            {
                defaultCullMode = windowLayerInfo.CullMode;
            }

            var showOption = new ShowWindowOption();
            showOption.CullMode = option.CullMode.IsNull ? defaultCullMode : option.CullMode;
            showOption.IsShowMask = option.IsShowMask.IsNull ? windowInfo.IsShowMask : option.IsShowMask;
            showOption.MaskName = option.MaskName ?? windowInfo.MaskName;
            showOption.MaskAlpha = option.MaskAlpha.IsNull ? 1f : option.MaskAlpha;
            showOption.IsIgnoreCull = option.IsIgnoreCull.IsNull ? false : option.IsIgnoreCull;
            showOption.IsCloseByClickBlank = option.IsCloseByClickBlank.IsNull
                ? windowInfo.IsCloseByClickBlank
                : option.IsCloseByClickBlank;

            return showOption;
        }

        //
        private void AddWindowToStack(long instanceId, UIWindow uiWindow)
        {
            var windowName = uiWindow.WindowName;
            if (!this.m_visibleWindowByNameDict.TryGetValue(windowName, out var windowStack))
            {
                windowStack = new Stack<long>();
                this.m_visibleWindowByNameDict.Add(windowName, windowStack);
            }

            windowStack.Push(instanceId);
            this.m_visibleWindowDict.Add(instanceId, uiWindow);
        }

        //
        private void RemoveWindowFromStack(long instanceId)
        {
            if (!this.m_visibleWindowDict.TryGetValue(instanceId, out var uiWindow))
            {
                return;
            }

            var windowName = uiWindow.WindowName;
            if (!this.m_visibleWindowByNameDict.TryGetValue(windowName, out var windowStack))
            {
                return;
            }

            windowStack.Pop();
            this.m_visibleWindowDict.Remove(instanceId);
        }

        private UIWindow GetWindowByName(string windowName)
        {
            if (!this.m_visibleWindowByNameDict.TryGetValue(windowName, out var windowStack))
            {
                return null;
            }

            if (windowStack.Count <= 0)
            {
                return null;
            }

            return GetWindow(windowStack.Peek());
        }

        private UIWindow GetWindow(long instanceId)
        {
            if (this.m_visibleWindowDict.TryGetValue(instanceId, out var uiWindow))
            {
                return uiWindow;
            }

            return null;
        }

        #endregion

        #region 界面显示

        public bool IsWindowOpened<TWindow>() where TWindow : UIWindow
        {
            var windowName = typeof(TWindow).Name;
            var uiWindow = this.GetWindowByName(windowName);
            return uiWindow != null;
        }

        public long GetWindowInstanceId<TWindow>() where TWindow : UIWindow
        {
            var windowName = typeof(TWindow).Name;
            var uiWindow = this.GetWindowByName(windowName);
            if (uiWindow == null)
                return -1;
            return uiWindow.InstanceId;
        }

        public async UniTask<long> OpenWindowAsync<TWindow>(IUIShowPara showPara = null,
            ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow
        {
            var windowType = typeof(TWindow);
            var windowName = windowType.Name;
            var uiWindow =
                await OpenWindowInternal(windowName, windowType, false, showPara, showOption, cancellationToken);
            if (uiWindow == null)
                return -1;
            return uiWindow.InstanceId;
        }

        public async UniTask<long> OpenWindowInstanceAsync<TWindow>(IUIShowPara showPara = null,
            ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default) where TWindow : UIWindow
        {
            var windowType = typeof(TWindow);
            var windowName = windowType.Name;
            var uiWindow =
                await OpenWindowInternal(windowName, windowType, true, showPara, showOption, cancellationToken);
            if (uiWindow == null)
                return -1;
            return uiWindow.InstanceId;
        }

        private async UniTask<UIWindow> OpenWindowInternal(string windowName, Type windowType, bool isInstance,
            IUIShowPara showPara = null, ShowWindowOption showOption = default,
            CancellationToken cancellationToken = default)
        {
            await this.CheckConfigLoaded();
            var uiWindow = await this.CreateWindowAsync(windowName, windowType, isInstance, cancellationToken);
            if (uiWindow == null)
                return null;
            uiWindow.SetShowWindowOption(CombineWindowShowOption(uiWindow.WindowInfo, showOption));
            uiWindow.ForceShow(showPara);
            m_needResort = true;
            //TODO:处理开启窗口的动画
            return uiWindow;
        }

        #endregion

        #region 界面隐藏

        public void CloseWindow<TWindow>() where TWindow : UIWindow
        {
            var uiWindow = GetWindowByName(typeof(TWindow).Name);
            if (uiWindow == null)
                return;
            CloseWindowInstance(uiWindow.InstanceId);
        }

        public void CloseWindowInstance(long instanceId)
        {
            var uiWindow = GetWindow(instanceId);
            if (uiWindow == null)
                return;
            uiWindow.Hide();
            RemoveWindow(instanceId);
            this.m_needResort = true;
        }

        #endregion

        #region 界面排序

        private void SortWindows()
        {
            this.m_visibleSortedWindows.Clear();
            if (this.m_visibleWindowDict.Count > 0)
            {
                foreach (var uiWindow in this.m_visibleWindowDict.Values)
                {
                    this.m_visibleSortedWindows.Add(uiWindow);
                }

                this.m_visibleSortedWindows.Sort(SortWindowMethod);
                int sortingOrder = 1;
                foreach (var uiWindow in this.m_visibleSortedWindows)
                {
                    uiWindow.SetSortingOrder(sortingOrder);
                    sortingOrder += uiWindow.SortRange + 3; //1层留给遮罩 2层留给空白
                }
            }

            this.UpdateVisibility();
            this.UpdateMask();
            this.UpdateBlank();
        }

        private int SortWindowMethod(UIWindow a, UIWindow b)
        {
            if (a.SortValue != b.SortValue)
            {
                return a.SortValue.CompareTo(b.SortValue);
            }

            return a.OpenTime.CompareTo(b.OpenTime);
        }

        //更新遮罩
        private void UpdateMask()
        {
            int count = this.m_finallySortedWindows.Count;
            var isShowMask = false;
            var maskName = string.Empty;
            var maskAlpha = 1f;
            var sortingOrder = 0;
            //从顶层找到第一个有遮罩的界面
            for (int i = count - 1; i >= 0; --i)
            {
                var uiWindow = this.m_finallySortedWindows[i];
                if (uiWindow.IsShowMask)
                {
                    isShowMask = true;
                    maskName = uiWindow.MaskName;
                    maskAlpha = uiWindow.MaskAlpha;
                    sortingOrder = uiWindow.Canvas.sortingOrder;
                    break;
                }
            }

            if (isShowMask)
            {
                //显示遮罩
                this.ShowMask(maskName, sortingOrder - 2, maskAlpha);
            }
            else
            {
                //隐藏遮罩
                this.HideMask();
            }
        }

        //更新Blank区域
        private void UpdateBlank()
        {
            int count = this.m_finallySortedWindows.Count;
            var ownerInstanceId = -1L;
            var sortingOrder = 0;
            //从顶层找到第一个需要点击空白处关闭的界面
            for (int i = count - 1; i >= 0; --i)
            {
                var uiWindow = this.m_finallySortedWindows[i];
                if (uiWindow.IsCloseByClickBlank)
                {
                    ownerInstanceId = uiWindow.InstanceId;
                    sortingOrder = uiWindow.Canvas.sortingOrder;
                    break;
                }
            }

            if (ownerInstanceId <= 0)
            {
                this.m_blankRoot.gameObject.SetActive(false);
            }
            else
            {
                this.m_currentBlankOwnerInstanceId = ownerInstanceId;
                this.m_blankCanvas.sortingOrder = sortingOrder - 1;
                this.m_blankRoot.gameObject.SetActive(true);
            }
        }

        //显示遮罩
        private void ShowMask(string maskName, int sortingOrder, float alpha)
        {
            if (this.m_currentMask != null && this.m_currentMask.name == maskName)
            {
                this.m_maskCanvas.sortingOrder = sortingOrder;
                return;
            }

            this.HideCustomMask();
            if (!string.IsNullOrEmpty(maskName) && maskName.StartsWith("Mask"))
            {
                this.ShowCustomMask(maskName, this.m_uniTaskCTS.Create("ShowMask").Token).Forget();
            }

            this.m_maskRoot.gameObject.SetActive(true);
            this.m_maskCanvas.sortingOrder = sortingOrder;
            this.m_maskRoot.GetComponent<CanvasGroup>().alpha = alpha;
        }

        //隐藏遮罩
        private void HideMask()
        {
            this.HideCustomMask();
            this.m_maskRoot.gameObject.SetActive(false);
        }

        //更新界面可见性
        private void UpdateVisibility()
        {
            this.m_finallySortedWindows.Clear();
            UIWindow lastWindow = null;
            int count = this.m_visibleSortedWindows.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                var currWindow = this.m_visibleSortedWindows[i];
                var isCulled = this.IsWindowCulled(currWindow, lastWindow);
                if (isCulled)
                {
                    currWindow.Hide();
                }
                else
                {
                    currWindow.Show();
                    lastWindow = currWindow;
                    this.m_finallySortedWindows.Insert(0, currWindow);
                }
            }
        }

        //判断界面是否被剔除
        private bool IsWindowCulled(UIWindow currWindow, UIWindow lastWindow)
        {
            //第一个界面不需要剔除
            if (lastWindow == null)
            {
                return false;
            }

            //无视剔除
            if (currWindow.Option.IsIgnoreCull)
            {
                return false;
            }

            //所有都被剔除
            if ((lastWindow.Option.CullMode & EUICullMode.All) != 0)
            {
                return true;
            }

            //同层才被剔除
            if ((lastWindow.Option.CullMode & EUICullMode.OnlySameLayer) != 0)
            {
                if (currWindow.SortLayer == lastWindow.SortLayer)
                {
                    return true;
                }
            }

            return false;
        }

        private async UniTask ShowCustomMask(string maskName, CancellationToken cancellationToken)
        {
            try
            {
                var result = await this.m_resLoader.InstantiateAsync(maskName, this.m_maskRoot, cancellationToken)
                    .SuppressCancellationThrow();
                if (result.IsCanceled)
                    return;
                this.m_currentMask = result.Result;
                this.m_currentMask.name = maskName;
                //Debug.Log($"加载完成遮罩{maskName}");
            }
            catch (Exception e)
            {
                Debug.Log($"未找到遮罩{maskName},使用默认Mask");
            }
        }

        private void HideCustomMask()
        {
            if (this.m_currentMask != null)
            {
                //Debug.Log($"卸载完成遮罩{self.CurrentMask.name}");
                this.m_resLoader.ReleaseInstance(this.m_currentMask);
                this.m_currentMask = null;
            }
        }

        #endregion
    }
}