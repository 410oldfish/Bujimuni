using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;


public abstract class ControllerGearTreeItemBase: ControllerTreeItemBase
{

    public const int NEW_PAGE_INDEX = 10000;

    public const int MONITOR_INDEX = -2;

    protected GearTypeState _gear;

    public GearTypeState Gear
    {
        get
        {
            return _gear;
        }

        set
        {
            _gear = value;
        }
    }

    public GameObject Target
    {
        get
        {
            return (_parent as ControllerGameObjectTreeItem).Target;
        }
    }

    public int SelectedIndex
    {
        get
        {
            return StatControllerManager.Instance.CurControllerWrapper.SelectedIndex;
        }
    }

    public StatController UIScript
    {
        get
        {
            return StatControllerManager.Instance.CurStatController;
        }
    }

    public StatControllerControllerWrapper ControllerWrapper
    {
        get
        {
            return StatControllerManager.Instance.CurControllerWrapper;
        }
    }

    public ControllerGearTreeItemBase(GearTypeState gearType)
    {
        _type = ControllerTreeItemType.Gear;
        _gear = gearType;
    }

    public override void OnHeaderGUI(int index, Rect itemArea)
    {
        base.OnHeaderGUI(index, itemArea);
        Rect tagArea = new Rect(itemArea.x - 6 + DepthValue * 20,
            itemArea.y + (_height - EditorGUIUtility.singleLineHeight) / 2,
            140,
            EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(tagArea, _gear.ToString());
    }

    public override void OnAddGearGUI(int index, Rect itemArea)
    {
        base.OnAddGearGUI(index, itemArea);
        Rect tagArea = new Rect(itemArea.x - 6 + DepthValue * 20,
            itemArea.y + (_addGearHeight - EditorGUIUtility.singleLineHeight) / 2,
            140,
            EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(tagArea, _gear.ToString());

        Rect addBtnArea = new Rect(itemArea.xMax - 20,
            itemArea.y + (_addGearHeight - EditorGUIUtility.singleLineHeight) / 2,
            20,
            EditorGUIUtility.singleLineHeight);
        if (GUI.Button(addBtnArea, "+", EditorStyles.toolbarButton))
        {
            State = ControllerTreeItemState.Show;
            StatControllerManager.Instance.CurControllerWrapper.RebuildShowTreeList();
            StatControllerManager.Instance.ControllerSettings.InAddNewGearState = false;
        }
    }

    protected override void OnSelfStateChange(ControllerTreeItemState state)
    {
        if (state == ControllerTreeItemState.Show)
        {
            Init();
        }
    }

    public override void OnAddNewPage()
    {
        if (_state == ControllerTreeItemState.Show)
        {
            RecordValue();
        }

        base.OnAddNewPage();
    }

    public override void Apply()
    {
        if (_state == ControllerTreeItemState.Show)
        {
            ApplyValue(SelectedIndex);
        }

        base.Apply();
    }

    public virtual void Init()
    {
        for (int i = 0; i < ControllerWrapper.PageNameList.Count; i++)
        {
            RecordValue();
        }
    }

    protected virtual void OnValueChanged()
    {
        if (StatControllerManager.Instance.ControllerSettings.AutoApply)
        {
            StatControllerManager.Instance.CurControllerWrapper.Apply();
        }

        if (StatControllerManager.Instance.WindowSettings.AutoSave)
        {
            string failedStr = null;
            if (StatControllerManager.Instance.CurStatControllerWrapper.CheckCanSave(true, out failedStr))
            {
                StatControllerManager.Instance.CurStatControllerWrapper.Save();
            }
            else
            {
                Debug.Log(string.Format("{0}原因: {1}", "保存失败,已退出自动保存模式", failedStr)); // EditorUtility.DisplayDialog("保存失败", failedStr, "确认");
            }
        }

        StatControllerManager.Instance.NeedRepaint = true;
    }

    public override void RebuildGearConfigList(List<GearConfig> gearConfigList, StatControllerStoredDataBuilder dataBuilder)
    {
        if (_state == ControllerTreeItemState.Hide)
        {
            return;
        }

        gearConfigList.Add(BuildConfig(dataBuilder));
        base.RebuildGearConfigList(gearConfigList, dataBuilder);
    }

    public abstract void LoadConfig(GearConfig config);

    public abstract GearConfig BuildConfig(StatControllerStoredDataBuilder dataBuilder);

    public abstract void RecordValue(int pageIndex = NEW_PAGE_INDEX);

    public abstract void ApplyValue(int pageIndex);

    public abstract void MonitorValueChange();
}