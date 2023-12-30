using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class SimpleUseSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartTask().Forget();
    }

    // Update is called once per frame
    void Update()
    {

    }

    async UniTask StartTask()
    {
        Debug.Log("1111");
        await UniTask.Delay(new TimeSpan(0, 0, 1));
        Debug.Log("2222");
        await UniTask.Delay(new TimeSpan(0, 0, 1));
        Debug.Log("3333");
        await UniTask.Delay(new TimeSpan(0, 0, 1));
        Debug.Log("4444");
        await UniTask.Delay(new TimeSpan(0, 0, 1));
    }

}
