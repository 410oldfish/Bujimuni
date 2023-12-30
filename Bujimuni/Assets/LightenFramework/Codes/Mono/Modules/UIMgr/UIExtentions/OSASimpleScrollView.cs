using System;
using System.Collections;
using System.Collections.Generic;
using Com.TheFallenGames.OSA.Core;
using Com.TheFallenGames.OSA.CustomParams;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OSASimpleScrollView : OSA<OSASimpleScrollView.MyParams, OSASimpleScrollView.MyItemViewsHolder>
{
#if UNITY_EDITOR
    
    [LabelText("预览项数量")]
    public int previewItemsCount = 10;
    
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
        RemovePreviewObjs();
        ClearVisibleItems();
        ClearCachedRecyclableItems();
    }
    
#endif

    [HideInInspector] public UnityEvent<MyItemViewsHolder> onCreateItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<MyItemViewsHolder> onRemoveItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<MyItemViewsHolder> onUpdateItem = new UnityEvent<MyItemViewsHolder>();
    [HideInInspector] public UnityEvent<double> onScrollPositionChanged = new UnityEvent<double>();

    [HideInInspector]
    public UnityEvent<MyItemViewsHolder, double, double> onApplyCustomGalleryEffects =
        new UnityEvent<MyItemViewsHolder, double, double>();

    [HideInInspector]
    public UnityEvent<MyItemViewsHolder> onRemoveCustomGalleryEffects = new UnityEvent<MyItemViewsHolder>();

    protected override void Awake()
    {
        base.Awake();

        if (IsPreview())
        {
            RemovePreviewObjs();
        }
    }

    protected override MyItemViewsHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new MyItemViewsHolder();
        instance.Init(_Params.ItemPrefab, _Params.Content, itemIndex);
        onCreateItem.Invoke(instance);
        return instance;
    }

    protected override void OnBeforeDestroyViewsHolder(MyItemViewsHolder vh, bool isActive)
    {
        base.OnBeforeDestroyViewsHolder(vh, isActive);
        onRemoveItem.Invoke(vh);
    }

    protected override void UpdateViewsHolder(MyItemViewsHolder newOrRecycled)
    {
        //Debug.Log($"UpdateViewsHolder {newOrRecycled.ItemIndex}");
        onUpdateItem.Invoke(newOrRecycled);
    }

    protected override void OnApplyCustomGalleryEffects(MyItemViewsHolder vh, double itemCenterDistFromStart01,
        double itemCenterPosRelativeToPivot01Signed)
    {
        base.OnApplyCustomGalleryEffects(vh, itemCenterDistFromStart01, itemCenterPosRelativeToPivot01Signed);
        onApplyCustomGalleryEffects.Invoke(vh, itemCenterDistFromStart01, itemCenterPosRelativeToPivot01Signed);
    }

    protected override void OnRemoveCustomGalleryEffects(MyItemViewsHolder vh)
    {
        base.OnRemoveCustomGalleryEffects(vh);
        onRemoveCustomGalleryEffects.Invoke(vh);
    }

    protected override void OnScrollPositionChanged(double normPos)
    {
        base.OnScrollPositionChanged(normPos);
        onScrollPositionChanged?.Invoke(normPos);
    }

    public List<MyItemViewsHolder> GetItems()
    {
        return this._VisibleItems;
    }

    [Serializable]
    public class MyParams : BaseParamsWithPrefab
    {
    }

    public class MyItemViewsHolder : BaseItemViewsHolder
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
        foreach (var child in removed)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}