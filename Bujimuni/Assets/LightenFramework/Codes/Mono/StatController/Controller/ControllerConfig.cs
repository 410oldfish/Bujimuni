using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ControllerConfig
{
    [SerializeField]
    public string name;

    [SerializeField]
    public int selectedIndex;

    [SerializeField]
    public string[] pageNames;

    [SerializeField]
    public GearConfig[] gearConfigs;

    [SerializeField]
    public string[] pageTips;
}