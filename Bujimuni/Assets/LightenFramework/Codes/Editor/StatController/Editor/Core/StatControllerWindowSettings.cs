using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StatControllerWindowSettings
{
    private StatControllerPanelBase[] _panels;

    private int _curPanelIndex = 0;

    private bool _autoSave = false;

    private float _scrollWheelSteps = 4;

    private StatControllerControllerPanelSettings _controllerSettings;
    public int CurPanelIndex
    {
        get
        {
            return 0;
        }

        set
        {
            if (_curPanelIndex == value)
            {
                return;
            }

            _curPanelIndex = value;
            /*EditorPrefs.SetInt("StatControllerWindowSettings.CurPanelIndex", _curPanelIndex);*/
            StatControllerManager.Instance.NeedRepaint = true;
        }
    }

    public StatControllerPanelBase[] Panels
    {
        get
        {
            return _panels;
        }

        set
        {
            _panels = value;
        }
    }

    public StatControllerControllerPanelSettings ControllerSettings
    {
        get
        {
            return _controllerSettings;
        }

        set
        {
            _controllerSettings = value;
        }
    }

    public bool AutoSave
    {
        get => _autoSave;
        set => _autoSave = value;
    }

    public float MouseScrollWheelSteps
    {
        get => _scrollWheelSteps;
        set => _scrollWheelSteps = value;
    }
    

    public StatControllerWindowSettings()
    {
        _panels = new StatControllerPanelBase[]
        {
            new StatControllerControllerPanel(null)
        };
        /*if (EditorPrefs.HasKey("StatControllerWindowSettings.CurPanelIndex"))
        {
            _curPanelIndex = 0;
        }
        else
        {
            _curPanelIndex = 0;
        }*/

        _controllerSettings = new StatControllerControllerPanelSettings();
        Init();
        _panels[_curPanelIndex].OnEnter();
    }

    public void Init()
    {
        _controllerSettings.Init();
    }

    public void Refresh()
    {
        _controllerSettings.Refresh();
    }
}