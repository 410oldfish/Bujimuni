using System.Collections.Generic;
using UnityEngine;

public class TextColorStyleGear: GearBase
{

    public TextColorStyleGear(Controller parent, GearConfig config): base(parent, config)
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

    }

    public override void Apply()
    {
        base.Apply();
        //_target.colorStyle = _values[_parent.SelectedIndex];
    }
}