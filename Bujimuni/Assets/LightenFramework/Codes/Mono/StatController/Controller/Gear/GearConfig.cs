using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GearConfig
{
    [SerializeField]
    public GearTypeState gearType;

    [SerializeField]
    public ushort[] dataArray;

    public ushort StoredGameObjectIndex
    {
        get
        {
            return dataArray[0];
        }
    }

    public GearConfig(GearTypeState type, List<ushort> datas)
    {
        gearType = type;
        dataArray = datas.ToArray();
    }
}