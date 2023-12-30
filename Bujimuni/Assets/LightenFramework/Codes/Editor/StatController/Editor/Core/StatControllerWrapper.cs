using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StatControllerWrapper
{
    private List<StatControllerControllerWrapper> _controllerWrapperList = new List<StatControllerControllerWrapper>();

    public List<StatControllerControllerWrapper> ControllerWrapperList
    {
        get
        {
            return _controllerWrapperList;
        }

        set
        {
            _controllerWrapperList = value;
        }
    }

    public StatControllerWrapper(StatController statController)
    {
        Init(statController);
    }

    public void Refresh()
    {
        for (int i = 0; i < _controllerWrapperList.Count; i++)
        {
            _controllerWrapperList[i].Refresh();
        }
    }

    private void Init(StatController statController)
    {
        _controllerWrapperList = new List<StatControllerControllerWrapper>();
        if (statController.ControllerConfigs != null && statController.ControllerConfigs.Length > 0)
        {
            for (int i = 0; i < statController.ControllerConfigs.Length; i++)
            {
                _controllerWrapperList.Add(new StatControllerControllerWrapper(statController.ControllerConfigs[i]));
            }
        }
    }

    public bool CheckCanSave(bool needCheckAllLabel, out string failedStr)
    {
        failedStr = "";

        return true;
    }

    public void Save()
    {
        if (StatControllerManager.Instance.CurStatController == null)
        {
            return;
        }

        StatControllerManager.Instance.CurStatController.ClearStoredDatas();
        StatControllerStoredDataBuilder dataBuilder = new StatControllerStoredDataBuilder();
        // 保存控制器信息
        if (_controllerWrapperList.Count > 0)
        {
            StatControllerManager.Instance.CurStatController.ControllerConfigs = new ControllerConfig[_controllerWrapperList.Count];
            for (int i = 0; i < _controllerWrapperList.Count; i++)
            {
                StatControllerManager.Instance.CurStatController.ControllerConfigs[i] = BuildControllerConfig(i, dataBuilder);
            }
        }
        
        //将处理好的临时数据存储保存回CurStatController上 p.s ref.CurStatController.gameObjects = data.gameObjects;
        dataBuilder.Store(StatControllerManager.Instance.CurStatController);
        //通知Unity将数据持久化保存下来
        EditorUtility.SetDirty(StatControllerManager.Instance.CurStatController);
    }

    private ControllerConfig BuildControllerConfig(int wrapperIndex, StatControllerStoredDataBuilder dataBuilder)
    {
        StatControllerControllerWrapper wrapper = _controllerWrapperList[wrapperIndex];
        ControllerConfig config = new ControllerConfig();
        config.name = wrapper.Name;
        config.pageNames = wrapper.PageNameList.ToArray();
        //标签备注
        config.pageTips = wrapper.PageTipsList.ToArray();
        config.selectedIndex = wrapper.SelectedIndex;
        config.gearConfigs = wrapper.RebuildGearConfigList(dataBuilder).ToArray();
        return config;
    }

}