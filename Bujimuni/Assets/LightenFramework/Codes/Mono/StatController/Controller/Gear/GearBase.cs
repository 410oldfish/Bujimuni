using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GearBase
{
    protected Controller _parent;

    protected bool _active;

    protected StatController Owner
    {
        get
        {
            return _parent.Owner;
        }
    }

    public GearBase(Controller parent, GearConfig config)
    {
        _parent = parent;
        Init(config);
    }

    public virtual void Apply()
    {
        if (!_active)
        {
            return;
        }
    }

    public abstract void Init(GearConfig config);
}