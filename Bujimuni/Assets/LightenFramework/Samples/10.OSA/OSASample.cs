using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class OSASample : MonoBehaviour
{
    public List<OSASimpleScrollView> scrollViews;
    public List<OSASimpleGridView> gridViews;
    //public ReactiveCollection<string> datas = new ReactiveCollection<string>();

    public int smoothScrollTo = 0;
    public float normalizedOffsetFromViewportStart = 0;
    public float normalizedPositionOfItemPivotToUse = 0;

    [Button("定位")]
    void SmoothScrollTo()
    {
        foreach (var scrollView in scrollViews)
        {
            scrollView.SmoothScrollTo(smoothScrollTo, 0.5f, normalizedOffsetFromViewportStart,
                normalizedPositionOfItemPivotToUse);
        }
        foreach (var gridView in gridViews)
        {
            gridView.SmoothScrollTo(smoothScrollTo, 0.5f, normalizedOffsetFromViewportStart,
                normalizedPositionOfItemPivotToUse);
        }
    }

    // Start is called before the first frame update
    async UniTaskVoid Start()
    {
        for (int i = 0; i < 500; ++i)
        {
            //datas.Add(i.ToString());
        }
        // datas.ObserveAdd().Subscribe((addData) =>
        // {
        //     Debug.Log($"add {addData} count:{datas.Count}");
        //     foreach (var scrollView in scrollViews)
        //     {
        //         scrollView.ResetItems(datas.Count);
        //     }
        //     foreach (var gridView in gridViews)
        //     {
        //         gridView.ResetItems(datas.Count);
        //     }
        // });
        // datas.ObserveRemove().Subscribe(removeData =>
        // {
        //     Debug.Log($"remove {removeData} count:{datas.Count}");
        //     foreach (var scrollView in scrollViews)
        //     {
        //         scrollView.ResetItems(datas.Count);
        //     }
        //     foreach (var gridView in gridViews)
        //     {
        //         gridView.ResetItems(datas.Count);
        //     }
        // });
        // foreach (var scrollView in scrollViews)
        // {
        //     scrollView.onCreateItem.AddListener(
        //         holder => { holder.btnClick.onClick.AddListener(() => { Debug.Log($"click {holder.ItemIndex}"); }); });
        //     scrollView.onUpdateItem.AddListener(holder =>
        //     {
        //         Debug.Log($"itemIndex:{holder.ItemIndex} count:{datas.Count}");
        //         var data = datas[holder.ItemIndex];
        //         holder.variables.GetValue<TMP_Text>("txtTitle").text = data;
        //     });
        //     scrollView.onApplyCustomGalleryEffects.AddListener((vh, d1, d2) =>
        //     {
        //         var d = Mathf.Sin((float)d1 * 5 * Mathf.PI);
        //         vh.variables.GetValue<RectTransform>("transView").localPosition = new Vector3(50 * d, 0, 0);
        //     } );
        //     scrollView.onRemoveCustomGalleryEffects.AddListener((vh) =>
        //     {
        //         vh.variables.GetValue<RectTransform>("transView").localPosition = Vector3.zero;
        //     });
        //     scrollView.Init();
        //     scrollView.ResetItems(datas.Count);
        // }
        // foreach (var gridView in gridViews)
        // {
        //     gridView.onCreateItem.AddListener(
        //         holder => { holder.btnClick.onClick.AddListener(() => { Debug.Log($"click {holder.ItemIndex}"); }); });
        //     gridView.onUpdateItem.AddListener(holder =>
        //     {
        //         Debug.Log($"itemIndex:{holder.ItemIndex} count:{datas.Count}");
        //         var data = datas[holder.ItemIndex];
        //         holder.variables.GetValue<TMP_Text>("txtTitle").text = data;
        //     });
        //     gridView.Init();
        //     gridView.ResetItems(datas.Count);
        // }

        await UniTask.CompletedTask;
    }
}