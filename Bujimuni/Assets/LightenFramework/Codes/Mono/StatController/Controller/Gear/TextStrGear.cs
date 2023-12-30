using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextStrGear: GearBase
{
    private Text _target;

    private string[] _values;

    public TextStrGear(Controller parent, GearConfig config): base(parent, config)
    {
    }

    public override void Init(GearConfig config)
    {
        GameObject go = Owner.GetStoredGameObject(config.StoredGameObjectIndex);
        if (go == null)
        {
            _active = false;
            return;
        }

        _target = go.GetComponent<Text>();
        _active = _target != null;
        if (!_active)
        {
            return;
        }

        _values = new string[config.dataArray.Length - 1];
        for (int i = 1; i < config.dataArray.Length; i++)
        {
            _values[i - 1] = Owner.StoredStrings[config.dataArray[i]];
        }
    }

    public override void Apply()
    {
        base.Apply();
        _target.text = _values[_parent.SelectedIndex];
    }
}