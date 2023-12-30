using System;
using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class OSAMultiGroupGridView: GridAdapter<OSAMultiGroupGridView.MyParams, OSAMultiGroupGridView.MyItemViewsHolder>
{
    #region 预览

#if UNITY_EDITOR

    [LabelText("预览数据")]
    public List<int> GroupCountList = new List<int>();

    [HideIf("IsPreview")]
    [Button("开 启 预 览", ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
    void Preview()
    {
        StopPreview();
        Init();
        this.ClearGroups();
        for (int i = 0; i < GroupCountList.Count; i++)
        {
            this.AddGroup(i, GroupCountList[i]);
        }

        this.ResetGroupItems();
    }

    [ShowIf("IsPreview")]
    [Button("关 闭 预 览", ButtonSizes.Gigantic), GUIColor(1, 0, 0)]
    void StopPreview()
    {
        ClearVisibleItems();
        ClearCachedRecyclableItems();
        RemovePreviewObjs();
        this.ClearGroups();
        var p = _Params;
        Dispose();
        _Params = p;
    }

#endif

    protected override void Awake()
    {
        base.Awake();

        if (IsPreview())
        {
            RemovePreviewObjs();
        }
    }

    private bool IsPreview()
    {
        if (Content == null)
            return false;
        return Content.childCount > 0;
    }

    private void RemovePreviewObjs()
    {
        var removed = new List<Transform>();
        if (Content != null)
        {
            foreach (Transform child in Content)
            {
                removed.Add(child);
            }
        }

        var cellGroupPrefab = transform.Find(_Params.GetCellGroupPrefabName(gameObject.name));
        if (cellGroupPrefab != null)
        {
            removed.Add(cellGroupPrefab);
        }

        foreach (var child in removed)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    #endregion

    //分组数据
    private Dictionary<int, int> m_groupNums = new Dictionary<int, int>();

    public struct CellData
    {
        public static CellData Empty { get; private set; } = new CellData(-1);
        public int GroupId;
        public int IndexInGroup;
        public bool IsHeader;

        public CellData(int groupId)
        {
            this.GroupId = groupId;
            this.IsHeader = false;
            this.IndexInGroup = -1;
        }
    }

    private List<CellData> m_cellDatas = new List<CellData>();

    public void ClearGroups()
    {
        this.m_groupNums.Clear();
        this.m_cellDatas.Clear();
    }

    public void AddGroup(int groupId, int itemsCount)
    {
        this.m_groupNums[groupId] = itemsCount;
    }

    public CellData GetCellData(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= this.m_cellDatas.Count)
            return CellData.Empty;
        return this.m_cellDatas[itemIndex];
    }

    public void ResetGroupItems()
    {
        var curGroupCellNum = this._Params.CurrentUsedNumCellsPerGroup;
        m_cellDatas.Clear();
        foreach (var pairs in this.m_groupNums)
        {
            var groupId = pairs.Key;
            var groupNum = pairs.Value;
            for (int i = 0; i < curGroupCellNum; ++i)
            {
                this.m_cellDatas.Add(new CellData() { GroupId = groupId, IsHeader = true, IndexInGroup = -1, });
            }

            for (int i = 0; i < groupNum; ++i)
            {
                this.m_cellDatas.Add(new CellData() { GroupId = groupId, IsHeader = false, IndexInGroup = i, });
            }

            while (this.m_cellDatas.Count % curGroupCellNum != 0)
            {
                this.m_cellDatas.Add(new CellData() { GroupId = groupId, IsHeader = false, IndexInGroup = -1, });
            }
        }

        ResetItems(this.m_cellDatas.Count);
    }

    [Serializable]
    public class MyParams: GridParams
    {
        public GameObject CellGroupPrefab;

        protected override GameObject CreateCellGroupPrefabGameObject()
        {
            return CellGroupPrefab;
        }

        protected override LayoutGroup AddLayoutGroupToCellGroupPrefab(GameObject cellGroupGameObject)
        {
            if (IsHorizontal)
            {
                var layoutGroup = cellGroupGameObject.GetComponent<VerticalLayoutGroup>();
                if (layoutGroup != null)
                    return layoutGroup;
                return cellGroupGameObject.AddComponent<VerticalLayoutGroup>(); // groups are columns in a horizontal scrollview
            }
            else
            {
                var layoutGroup = cellGroupGameObject.GetComponent<HorizontalLayoutGroup>();
                if (layoutGroup != null)
                    return layoutGroup;
                return cellGroupGameObject.AddComponent<HorizontalLayoutGroup>(); // groups are rows in a vertical scrollview
            }
        }
    }

    public class MyItemViewsHolder: CellViewsHolder
    {
        public int GroupId;
        public int IndexInGroup;
    }

    public class MyCellGroupViewsHolder: CellGroupViewsHolder<MyItemViewsHolder>
    {
        public ContentSizeFitter contentSizeFitterComponent;
        public GameObject Header;

        public override void CollectViews()
        {
            base.CollectViews();

            contentSizeFitterComponent = root.gameObject.GetComponent<ContentSizeFitter>();
            if (contentSizeFitterComponent == null)
            {
                contentSizeFitterComponent = root.gameObject.AddComponent<ContentSizeFitter>();
            }

            contentSizeFitterComponent.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitterComponent.enabled = true;

            var headerNode = this.root.Find("Header");
            if (headerNode == null)
            {
                throw new Exception($"OSAMultiGroup必须要有Header节点");
            }

            this.Header = headerNode.gameObject;
        }
    }

    [HideInInspector]
    public UnityEvent<int, GameObject> onUpdateGroupHeader = new UnityEvent<int, GameObject>();

    [HideInInspector]
    public UnityEvent<MyItemViewsHolder> onCreateItem = new UnityEvent<MyItemViewsHolder>();

    [HideInInspector]
    public UnityEvent<MyItemViewsHolder> onRemoveItem = new UnityEvent<MyItemViewsHolder>();

    [HideInInspector]
    public UnityEvent<MyItemViewsHolder> onUpdateItem = new UnityEvent<MyItemViewsHolder>();
    
    protected override CellGroupViewsHolder<MyItemViewsHolder> GetNewCellGroupViewsHolder()
    {
        return new MyCellGroupViewsHolder();
    }

    protected override void UpdateViewsHolder(CellGroupViewsHolder<MyItemViewsHolder> newOrRecycled)
    {
        base.UpdateViewsHolder(newOrRecycled);

        if (newOrRecycled.NumActiveCells > 0)
        {
            var firstCellVH = newOrRecycled.ContainingCellViewsHolders[0];
            var cellData = GetCellData(firstCellVH.ItemIndex);
            var newOrRecycledCasted = newOrRecycled as MyCellGroupViewsHolder;
            if (cellData.IsHeader)
            {
                newOrRecycledCasted.Header.SetActive(true);
                onUpdateGroupHeader.Invoke(cellData.GroupId, newOrRecycledCasted.Header);
            }
            else
            {
                newOrRecycledCasted.Header.SetActive(false);
            }
        }

        ScheduleComputeVisibilityTwinPass();
    }

    protected override void OnCellViewsHolderCreated(MyItemViewsHolder cellVH, CellGroupViewsHolder<MyItemViewsHolder> cellGroup)
    {
        onCreateItem.Invoke(cellVH);
    }

    protected override void OnBeforeDestroyViewsHolder(CellGroupViewsHolder<MyItemViewsHolder> vh, bool isActive)
    {
        for (int i = 0; i < vh.ContainingCellViewsHolders.Length; i++)
        {
            var cellVH = vh.ContainingCellViewsHolders[i];
            onRemoveItem.Invoke(cellVH);
        }

        base.OnBeforeDestroyViewsHolder(vh, isActive);
    }

    protected override void UpdateCellViewsHolder(MyItemViewsHolder viewsHolder)
    {
        var itemIndex = viewsHolder.ItemIndex;
        var cellData = GetCellData(itemIndex);
        if (cellData.GroupId == -1)
            return;
        if (cellData.IndexInGroup >= 0)
        {
            viewsHolder.views.gameObject.SetActive(true);
            viewsHolder.rootLayoutElement.ignoreLayout = false;
            viewsHolder.GroupId = cellData.GroupId;
            viewsHolder.IndexInGroup = cellData.IndexInGroup;
            onUpdateItem.Invoke(viewsHolder);
        }
        else
        {
            viewsHolder.views.gameObject.SetActive(false);
            viewsHolder.rootLayoutElement.ignoreLayout = true;
        }
    }
    
}