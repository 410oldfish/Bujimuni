using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Lighten;


public class GameLauncher : MonoBehaviour, IController
{
    public bool enablePredownload = false;
    public GameLauncherPanel gameLauncherPanel;

    private class Architecture : AbstractArchitecture, IAwake
    {
        public void Awake()
        {
            this.RegisterManager<IAssemblyMgr, AssemblyMgr>();
            this.RegisterManager<IResMgr, ResMgrByYooAsset>();
        }
    }

    private void Awake()
    {
        Game.InitArchitecture<Architecture>();
    }

    public IArchitecture GetArchitecture()
    {
        return Game.Architecture;
    }

    async UniTask Start()
    {
        Debug.Log($"platform: {Application.platform}");
        Debug.Log($"companyName: {Application.companyName}");
        Debug.Log($"productName: {Application.productName}");

        // if (enablePredownload)
        // {
        //     //第一步更新Addressable
        //     //await Addressables.InitializeAsync();
        //     AddressableHelper.OnDownloadProgress = (s, f) =>
        //     {
        //         gameLauncherPanel.SetDownloadProgress(f);
        //     };
        //     AddressableHelper.OnShowDownloadDialog = async (size) =>
        //     {
        //         Debug.Log($"需要更新... size:{GameUtils.GetByteText(size)}");
        //         return await gameLauncherPanel.ShowMessageBox("", $"download size = {GameUtils.GetByteText(size)}, are u sure?");
        //     };
        //     var successed = await AddressableHelper.InitializeAddressable();
        //     if (!successed)
        //     {
        //         return;
        //     }
        // }
        // else
        // {
        //     await Addressables.InitializeAsync();
        // }
        var resMgr = Game.Architecture.GetManager<IResMgr>();
        await resMgr.Predownload();
        var assemblyMgr = Game.Architecture.GetManager<IAssemblyMgr>();
        await assemblyMgr.Initialize();
        assemblyMgr.Run();
    }
}