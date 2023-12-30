using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public class GameHotfixMonoTemplateScript : HotfixMono
{
    public override void Awake()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono Awake");
    }

    public override void OnEnable()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnEnable");
    }

    public override void OnDisable()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnDisable");
    }

    public override UniTask Start()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono Start");
        return UniTask.CompletedTask;
    }

    public override void OnDestroy()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnDestroy");
    }

    public override void Update()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono Update");
    }

    public override void LateUpdate()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono LateUpdate");
    }

    public override void FixedUpdate()
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono FixedUpdate");
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnCollisionEnter2D");
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnCollisionStay2D");
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        Utility.Debug.Log(LightenConst.TAG, "hotfix mono OnCollisionExit2D");
    }
}
