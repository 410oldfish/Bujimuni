using System;
using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.CustomAdapters.GridView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class OSASimpleGridView : GridAdapter<OSASimpleGridView.MyParams, OSASimpleGridView.MyItemViewsHolder>
{
#if UNITY_EDITOR

    [LabelText("预览项数量")] public int previewItemsCount = 10;

    [HideIf("IsPreview")]
    [Button("开 启 预 览", ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
    void Preview()
    {
        StopPreview();
        Init();
        ResetItems(Mathf.Max(1, previewItemsCount));
    }

    [ShowIf("IsPreview")]
    [Button("关 闭 预 览", ButtonSizes.Gigantic), GUIColor(1, 0, 0)]
    void StopPreview()
    {
        ClearVisibleItems();
        ClearCachedRecyclableItems();
        RemovePreviewObjs();

        var p = _Params;
        Dispose();
        _Params = p;
    }

#endif

    [HideInInspector] public UnityEvent<MyItemViewsHolder> onCreateItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<MyItemViewsHolder> onRemoveItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<MyItemViewsHolder> onUpdateItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<PointerEventData> onDragOSAView = new UnityEvent<PointerEventData>();
    [HideInInspector] public UnityEvent<PointerEventData> onBeginDragView = new UnityEvent<PointerEventData>();
    [HideInInspector] public UnityEvent<PointerEventData> onEndDragView = new UnityEvent<PointerEventData>();

    protected override void Awake()
    {
        base.Awake();

        if (IsPreview())
        {
            RemovePreviewObjs();
        }
    }

    protected override void OnCellViewsHolderCreated(MyItemViewsHolder cellVH,
        CellGroupViewsHolder<MyItemViewsHolder> cellGroup)
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
        onUpdateItem.Invoke(viewsHolder);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        onDragOSAView.Invoke(eventData);
    }
    
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        onBeginDragView.Invoke(eventData);
    }
    
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        onEndDragView.Invoke(eventData);
    }

    [Serializable]
    public class MyParams : GridParams
    {
    }

    public class MyItemViewsHolder : CellViewsHolder
    {

        public override void CollectViews()
        {
            base.CollectViews();
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
}