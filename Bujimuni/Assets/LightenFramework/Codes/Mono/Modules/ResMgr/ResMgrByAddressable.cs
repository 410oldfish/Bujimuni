#if ADDRESSABLES_AVAILABLE

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Lighten
{
    public class ResMgrByAddressable : AbstractManager, IResMgr, IAwake, IDestroy
    {
        public void Awake()
        {
            // Addressables.InternalIdTransformFunc = location =>
            // {
            //     Debug.Log(location.InternalId);
            //     return location.InternalId;
            // };
        }
        
        public void Destroy()
        {
            Debug.Log("销毁ResMgrByAddressable");
        }
        
        public async UniTask Predownload()
        {
            Debug.Log("启动预下载");
            await UniTask.CompletedTask;
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken = default) where T : Object
        {
            var result = await Addressables.LoadAssetAsync<T>(assetName);
            if (cancellationToken.IsCancellationRequested)
            {
                Addressables.Release(result);
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }
            return result;
        }

        public void ReleaseAsset<T>(T asset) where T : Object
        {
            Addressables.Release(asset);
        }
        
        //实例化资源
        public async UniTask<GameObject> InstantiateAsync(string assetName, Transform parent = null, CancellationToken cancellationToken = default)
        {
            /*
            await Addressables.InstantiateAsync(assetName, parent).WithCancellation(cancellationToken);
            这里为什么不用WithCancellation?
            因为即使抛出的取消的异常,Addressable也会继续实例化,最终会导致这个实例变成没人控制的野人
            所以这里在Addressable完成后,判断这个token是否被取消,如果取消了,就释放这个实例
            */
            var result = await Addressables.InstantiateAsync(assetName, parent);
            if (cancellationToken.IsCancellationRequested)
            {
                //Debug.Log($"取消加载了 {result!=null}");
                Addressables.ReleaseInstance(result);
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }
            return result;
        }

        public void ReleaseInstance(GameObject go)
        {
            Addressables.ReleaseInstance(go);
        }
        
        public async UniTask<SceneInstance> LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, bool activateOnLoad = true,
            CancellationToken cancellationToken = default)
        {
            return await Addressables.LoadSceneAsync(sceneName, mode, activateOnLoad).WithCancellation(cancellationToken);
        }

        public async UniTask UnloadSceneAsync(SceneInstance sceneInstance, UnloadSceneOptions unloadSceneOptions,
            CancellationToken cancellationToken = default)
        {
            await Addressables.UnloadSceneAsync(sceneInstance).WithCancellation(cancellationToken);
        }
    }
}

#endif