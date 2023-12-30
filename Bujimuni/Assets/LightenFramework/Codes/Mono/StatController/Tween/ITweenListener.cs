using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITweenListener
{
    void OnTweenStart();

    void OnTweenUpdate();

    void OnTweenComplete();
}