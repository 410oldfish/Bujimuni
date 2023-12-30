using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighten
{
    public interface IUIShowPara
    {
    }

    public abstract class UIWidget : UIEntity, IDestroy
    {
        //显示状态
        public bool IsVisible { get; private set; }

        //资源
        public GameObject GameObject { get; private set; }
        public bool NeedUnloadGameObject { get; private set; }

        //自动化数据
        public EUIInitState InitState { get; private set; }
        public string DefaultParamText { get; private set; }

        //参数
        protected IUIShowPara m_showPara { get; private set; }

        //子组件缓存
        private Dictionary<string, UIWidget> m_widgetDict = new Dictionary<string, UIWidget>();

        //子节点缓存
        private Dictionary<string, XEntity> m_childrenDict = new Dictionary<string, XEntity>();

        public bool IsLoaded => this.GameObject != null;

        //构造默认参数
        public abstract IUIShowPara CreateDefaultPara(string defaultParaText);

        //监听事件
        protected abstract void RegisterEvent();

        //刷新内容
        protected abstract void RefreshUI();

        public override void Destroy()
        {
            base.Destroy();
            this.m_widgetDict.Clear();
            this.m_childrenDict.Clear();
            this.IsVisible = false;
            this.GameObject = null;
        }

        public void InitWidget(EUIInitState initState, string defaultParaText)
        {
            this.InitState = initState;
            this.DefaultParamText = defaultParaText;
        }

        protected virtual void OnLoad()
        {
            CreateChildren();
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        #region 加载资源

        public async UniTask<bool> LoadAsync(string assetName, Transform parent, CancellationToken cancellationToken)
        {
            var go = await this.ResLoader.InstantiateAsync(assetName, parent, cancellationToken);
            if (go == null)
            {
                Utility.Debug.LogError($"加载UI预制体失败: {assetName}");
                return false;
            }

            this.GameObject = go;
            this.NeedUnloadGameObject = true;
            this.OnLoad();
            return true;
        }

        public void Bind(GameObject go)
        {
            this.GameObject = go;
            this.NeedUnloadGameObject = false;
            this.OnLoad();
        }

        public void Unload()
        {
            if (this.NeedUnloadGameObject)
            {
                this.ResLoader.ReleaseInstance(this.GameObject);
                this.NeedUnloadGameObject = false;
            }

            this.GameObject = null;
        }

        #endregion

        #region 显示/隐藏

        public void ForceShow(IUIShowPara para = null)
        {
            this.Hide();
            this.Show(para);
        }

        public void Show(IUIShowPara para = null)
        {
            if (this.IsVisible)
                return;
            this.GameObject.SetActive(true);
            this.ShowChildWidget();

            if (this.m_showPara != para)
            {
                if (this.m_showPara != null)
                {
                    ObjectPool.Recycle(this.m_showPara);
                }

                this.m_showPara = para;
            }

            this.RegisterEvent();
            this.OnShow();
            this.RefreshUI();
            this.IsVisible = true;
        }

        public void Hide()
        {
            if (!this.IsVisible)
                return;
            this.HideChildWidget();
            this.GameObject.SetActive(false);
            this.UnRegisterAll();
            this.UniTaskCTS.StopAllTask();
            this.OnHide();
            if (this.m_showPara != null)
            {
                ObjectPool.Recycle(this.m_showPara);
                this.m_showPara = null;
            }
            this.IsVisible = false;
        }

        public T GetWidget<T>(string name) where T : UIEntity
        {
            if (this.m_widgetDict.ContainsKey(name))
            {
                return this.m_widgetDict[name] as T;
            }

            return null;
        }

        public T GetChild<T>(string name) where T:XEntity
        {
            if (this.m_childrenDict.ContainsKey(name))
            {
                return this.m_childrenDict[name] as T;
            }

            return null;
        }

        private void ShowChildWidget()
        {
            foreach (var childWgt in this.m_widgetDict.Values)
            {
                switch (childWgt.InitState)
                {
                    case EUIInitState.AutoHide:
                        childWgt.GameObject.SetActive(false);
                        break;
                    case EUIInitState.AutoShow:
                        if (string.IsNullOrEmpty(childWgt.DefaultParamText))
                        {
                            childWgt.Show();
                        }
                        else
                        {
                            childWgt.Show(childWgt.CreateDefaultPara(childWgt.DefaultParamText));
                        }

                        break;
                }
            }
        }

        private void HideChildWidget()
        {
            foreach (var childWgt in this.m_widgetDict.Values)
            {
                childWgt.Hide();
            }
        }

        #endregion

        #region 自动生成组件

        //生成GameObject下的子组件
        private void CreateChildren()
        {
            var nodeDatas = new List<(string, GameObject)>();
            this.FindAutoGenerateNodes(ref nodeDatas, this.GameObject.transform);
            foreach (var nodeData in nodeDatas)
            {
                this.CreateChildNode(nodeData.Item1, nodeData.Item2);
            }
        }

        private void FindAutoGenerateNodes(ref List<(string, GameObject)> nodeDatas, Transform root)
        {
            if (root.childCount <= 0)
            {
                return;
            }

            foreach (Transform child in root)
            {
                if (child.name.StartsWith(UIPrefix.Widget))
                {
                    nodeDatas ??= new List<(string, GameObject)>();
                    nodeDatas.Add((UIPrefix.Widget, child.gameObject));
                    continue;
                }

                if (child.name.StartsWith(UIPrefix.OSA))
                {
                    nodeDatas ??= new List<(string, GameObject)>();
                    nodeDatas.Add((UIPrefix.OSA, child.gameObject));
                    continue;
                }

                this.FindAutoGenerateNodes(ref nodeDatas, child);
            }
        }

        //创建节点实例
        private void CreateChildNode(string prefixName, GameObject gameObject)
        {
            switch (prefixName)
            {
                case UIPrefix.Widget:
                    if (this.m_widgetDict.ContainsKey(gameObject.name))
                    {
                        Debug.LogError($"已经存在同名Widget {gameObject.name}");
                        return;
                    }

                    var uiWidget = UIHelper.CreateWidgetByNode<UIWidget>(this, gameObject);
                    if (uiWidget == null)
                    {
                        return;
                    }

                    this.m_widgetDict[gameObject.name] = uiWidget;
                    break;
                case UIPrefix.OSA:
                {
                    var sv = gameObject.GetComponent<OSASimpleScrollView>();
                    if (sv != null)
                    {
                        var inst = this.AddChild<OSASimpleScrollViewComponent>();
                        inst.Init(sv);
                        this.m_childrenDict[gameObject.name] = inst;
                        return;
                    }

                    var gv = gameObject.GetComponent<OSASimpleGridView>();
                    if (gv != null)
                    {
                        var inst = this.AddChild<OSASimpleGridViewComponent>();
                        inst.Init(gv);
                        this.m_childrenDict[gameObject.name] = inst;
                        return;
                    }

                    var mgv = gameObject.GetComponent<OSAMultiGroupGridView>();
                    if (mgv != null)
                    {
                        var inst = this.AddChild<OSAMultiGroupGridViewComponent>();
                        inst.Init(mgv);
                        this.m_childrenDict[gameObject.name] = inst;
                        return;
                    }
                }
                    break;
            }
        }

        #endregion
    }

    public abstract class UIWidget<TShowPara> : UIWidget where TShowPara : class, IUIShowPara
    {
        public TShowPara Para => this.m_showPara as TShowPara;
    }
}