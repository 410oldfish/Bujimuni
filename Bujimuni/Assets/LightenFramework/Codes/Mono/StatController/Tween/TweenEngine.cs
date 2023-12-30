using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenEngine: MonoBehaviour
{
    void Update()
    {
        TweenManager.Instance.ApplicationUpdate();
    }
}