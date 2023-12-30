using System.Collections.Generic;
using UnityEngine;


public class TextFontStyleGear: GearBase
{

    public TextFontStyleGear(Controller parent, GearConfig config): base(parent, config)
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
        //_target.style = _values[_parent.SelectedIndex];
    }
}