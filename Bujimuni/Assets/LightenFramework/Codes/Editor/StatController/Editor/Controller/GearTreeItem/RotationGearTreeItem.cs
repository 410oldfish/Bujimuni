﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RotationGearTreeItem: ControllerGearTreeItemBase
{
    private Vector3 _monitorCheckValue;

    private List<Vector3> _values;

    public List<Vector3> Values
    {
        get
        {
            return _values;
        }

        set
        {
            _values = value;
        }
    }

    public RotationGearTreeItem(): base(GearTypeState.Rotation)
    {
        _values = new List<Vector3>();
    }

    public override void OnDetailGUI(int index, Rect itemArea)
    {
        base.OnDetailGUI(index, itemArea);
        float width = StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH - 20;
        for (int i = 0; i < _values.Count; i++)
        {
            Rect valueArea = new Rect(
                itemArea.x + StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH * i + (StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH - width) / 2,
                itemArea.y,
                width,
                EditorGUIUtility.singleLineHeight);
            Vector3 tempValue = EditorGUI.Vector3Field(valueArea, "", _values[i]);
            if (tempValue != _values[i])
            {
                _values[i] = tempValue;
                this.OnValueChanged();
            }
        }
    }

    public override void ApplyValue(int pageIndex)
    {
        Target.GetComponent<RectTransform>().localEulerAngles = (_values[pageIndex]);
    }

    public override GearConfig BuildConfig(StatControllerStoredDataBuilder dataBuilder)
    {
        return new GearConfig(_gear, dataBuilder.BuildDataList(Target, _values));
    }

    public override void LoadConfig(GearConfig config)
    {
        _values = new List<Vector3>();
        for (int i = 1; i < config.dataArray.Length; i += 3)
        {
            _values.Add(new Vector3(UIScript.StoredFloats[config.dataArray[i]],
                UIScript.StoredFloats[config.dataArray[i + 1]],
                UIScript.StoredFloats[config.dataArray[i + 2]]));
        }

        _state = ControllerTreeItemState.Show;
        State = ControllerTreeItemState.Show;
    }

    public override void MonitorValueChange()
    {
        if (Target.GetComponent<RectTransform>().localEulerAngles == _monitorCheckValue)
        {
            return;
        }

        bool stateChanged = _state == ControllerTreeItemState.Hide;
        if (stateChanged)
        {
            _values = new List<Vector3>();
            _state = ControllerTreeItemState.Show;
            State = ControllerTreeItemState.Show;
            for (int i = 0; i < ControllerWrapper.PageNameList.Count; i++)
            {
                if (i == ControllerWrapper.SelectedIndex)
                {
                    _values.Add(Target.GetComponent<RectTransform>().localEulerAngles);
                }
                else
                {
                    _values.Add(_monitorCheckValue);
                }
            }

            StatControllerManager.Instance.CurControllerWrapper.RebuildShowTreeList();
        }
        else
        {
            _values[SelectedIndex] = Target.GetComponent<RectTransform>().localEulerAngles;
        }

        _monitorCheckValue = Target.GetComponent<RectTransform>().localEulerAngles;
        this.OnValueChanged();
    }

    public override void OnRemovePage(int pageIndex)
    {
        if (_state == ControllerTreeItemState.Show)
        {
            _values.RemoveAt(pageIndex);
        }

        base.OnRemovePage(pageIndex);
    }

    public override void RecordValue(int pageIndex = NEW_PAGE_INDEX)
    {
        if (pageIndex == MONITOR_INDEX)
        {
            _monitorCheckValue = Target.GetComponent<RectTransform>().localEulerAngles;
        }
        else if (pageIndex == NEW_PAGE_INDEX)
        {
            _values.Add(Target.GetComponent<RectTransform>().localEulerAngles);
        }
    }

    public override void RefreshMonitorValue()
    {
        if (_state == ControllerTreeItemState.Show)
        {
            _monitorCheckValue = _values[SelectedIndex];
        }
        else
        {
            RecordValue(MONITOR_INDEX);
        }
    }
}
