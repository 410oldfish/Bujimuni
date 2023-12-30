using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class KeyFrameConfig
{
    [SerializeField]
    public LineTypeState lineType;

    [SerializeField]
    public ushort[] dataList;

    public KeyFrameConfig(LineTypeState inLineType, List<ushort> datas)
    {
        lineType = inLineType;
        dataList = datas.ToArray();
    }

    public ushort StoredGameObjectIndex
    {
        get
        {
            return dataList[0];
        }
    }


}