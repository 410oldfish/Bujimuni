using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller
{

    private string _name;

    private StatController _owner;

    private int _pageNum;

    private int _selectedIndex;

    private GearBase[] _gears;

    public int SelectedIndex
    {
        get
        {
            return _selectedIndex;
        }

        set
        {
            if (value == _selectedIndex)
            {
                return;
            }

            if (value < 0 || value >= _pageNum)
            {
                return;
            }

            _selectedIndex = value;
            Apply();
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }

    public StatController Owner
    {
        get
        {
            return _owner;
        }
    }

    public int PageNum
    {
        get
        {
            return _pageNum;
        }
    }

    public Controller(StatController owner, ControllerConfig config)
    {
        _owner = owner;
        _name = config.name;
        _pageNum = config.pageNames.Length;
        _selectedIndex = config.selectedIndex;
        _gears = new GearBase[config.gearConfigs.Length];
        for (int i = 0; i < config.gearConfigs.Length; i++)
        {
            _gears[i] = StatControllerUtility.GetGear(this, config.gearConfigs[i]);
        }
    }

    private void Apply()
    {
        if (_gears == null || _gears.Length == 0)
        {
            return;
        }

        for (int i = 0; i < _gears.Length; i++)
        {
            _gears[i].Apply();
        }
    }

    public void EditorApply(int selectedIndex)
    {
        _selectedIndex = selectedIndex;
        Apply();
    }
}