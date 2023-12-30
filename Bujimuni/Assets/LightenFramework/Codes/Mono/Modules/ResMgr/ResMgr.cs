using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighten
{
    public interface IResMgr
    {
        UniTask Predownload();
        
        //加载资源
        UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken = default) where T : Object;
        //释放资源
        void ReleaseAsset<T>(T asset) where T : Object;

        //实例化GameObject
        UniTask<GameObject> InstantiateAsync(string assetName, Transform parent = null,
            CancellationToken cancellationToken = default);
        //销毁并释放GameObject
        void ReleaseInstance(GameObject go);
        
        //加载场景
        UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool activateOnLoad = true,
            CancellationToken cancellationToken = default);
        //卸载场景
        UniTask UnloadSceneAsync(string sceneName, UnloadSceneOptions unloadSceneOptions, CancellationToken cancellationToken = default);
    }

    // public class ResMgr : AbstractManager, IResMgr, IDestroy
    // {
    //     public void Predownload()
    //     {
    //         Debug.Log("启动预下载");
    //     }
    //
    //     public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken = default) where T : Object
    //     {
    //         Debug.Log($"加载资源{assetName}");
    //         await UniTask.CompletedTask;
    //         return null;
    //     }
    //
    //     //实例化资源
    //     public async UniTask<GameObject> InstantiateAsync(string assetName, Transform parent = null, CancellationToken cancellationToken = default)
    //     {
    //         Debug.Log($"实例化资源{assetName}");
    //         await UniTask.CompletedTask;
    //         return null;
    //     }
    //
    //
    //     public void Destroy()
    //     {
    //         Debug.Log("销毁ResourceMgr");
    //     }
    // }
}