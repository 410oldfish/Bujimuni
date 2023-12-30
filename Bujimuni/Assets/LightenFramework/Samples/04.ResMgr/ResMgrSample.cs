using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;
using UnityEngine.UI;

public class ResMgrSample : MonoBehaviour, IController
{
    private class Architecture : AbstractArchitecture, IAwake
    {
        public void Awake()
        {
            this.RegisterManager<IResMgr, ResMgrByYooAsset>();
        }
    }
    
    public Image image;
    public string spriteName;

    private void Awake()
    {
        Game.InitArchitecture<Architecture>();
    }

    async void Start()
    {
        var resMgr = this.GetManager<IResMgr>();
        var sprite = await resMgr.LoadAssetAsync<Sprite>(spriteName);
        image.sprite = sprite;
        await UniTask.WaitForSeconds(2f);
        //这一步只能在Use Existing Mode 才能看到效果
        resMgr.ReleaseAsset(sprite);
    }

    public IArchitecture GetArchitecture()
    {
        return Game.Architecture;
    }
}
