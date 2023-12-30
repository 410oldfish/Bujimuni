using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Lighten;


public class UIMgrSample : XEntityMonoBehaviour<GameEntity>, IController
{
    private class Architecture : AbstractArchitecture, IAwake
    {
        public void Awake()
        {
            this.RegisterManager<IResMgr, ResMgrByYooAsset>();
            this.RegisterManager<IUIMgr, UIMgr>();
            this.RegisterManager<IAssemblyMgr, AssemblyMgr>();
        }
    }

    protected override void OnEntityAwake()
    {
        base.OnEntityAwake();
        Game.InitArchitecture<Architecture>();
    }
    
    void Start()
    {
        this.Entity.CurrentUI().OpenWindowAsync<DlgSampleUI01>().Forget();
    }

    private void Update()
    {
        
    }

    public IArchitecture GetArchitecture()
    {
        return Game.Architecture;
    }
}