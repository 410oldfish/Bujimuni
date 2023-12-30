using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TransitionConfig
{
    [SerializeField]
    public string name;

    [SerializeField]
    public bool autoPlay;

    [SerializeField]
    public int autoPlayTimes;

    [SerializeField]
    public float autoPlayDelay;

    [SerializeField]
    public KeyFrameConfig[] keyFrameConfigs;
}