using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveKeyFrame: ValuesKeyFrameBase
{
    private GameObject _target;

    public ActiveKeyFrame(Transition parent, KeyFrameConfig config): base(parent, config)
    {
    }

    public override void Init(KeyFrameConfig config)
    {
        _target = Owner.GetStoredGameObject(config.StoredGameObjectIndex);
        if (_target == null)
        {
            _active = false;
            return;
        }

        _active = true;
        _valueSize = 1;
        _frameTime = (float)Owner.StoredInts[config.dataList[1]] / 30f;
        int checkNum = 2;
        _values = new float[_valueSize];
        for (int i = 0; i < _valueSize; i++)
        {
            _values[i] = config.dataList[checkNum++];
        }

        _actives = new bool[_valueSize];
        for (int i = 0; i < _valueSize; i++)
        {
            _actives[i] = config.dataList[checkNum++] == StatControllerUtility.TrueValue;
        }
    }

    public override void ApplyValue()
    {
        if (_target != null)
        {
            _target.SetActive(_values[0] == StatControllerUtility.TrueValue);
        }
    }
}