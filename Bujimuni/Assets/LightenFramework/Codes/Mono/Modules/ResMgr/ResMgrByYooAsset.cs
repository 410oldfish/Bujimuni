using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Lighten
{
    public class ResMgrByYooAsset : AbstractManager, IResMgr, IAwake, IDestroy, IUpdate
    {
        private ResourcePackage m_defaultPackage;

        private Dictionary<Object, AssetHandle> m_assetHandleDict = new Dictionary<Object, AssetHandle>();
        private Dictionary<string, SceneHandle> m_sceneHandleDict = new Dictionary<string, SceneHandle>();
        private float m_unloadDelay = 10f;//10秒钟清理一次未使用资源
        private float m_unloadDelayCounter = 0f;

        public void Awake()
        {
            YooAssets.Initialize();
            m_defaultPackage = YooAssets.CreatePackage("DefaultPackage");
            YooAssets.SetDefaultPackage(m_defaultPackage);
        }

        public void Destroy()
        {
        }

        public void Update(float elapsedTime)
        {
            if (m_defaultPackage != null)
            {
                m_unloadDelayCounter += elapsedTime;
                if (m_unloadDelayCounter >= this.m_unloadDelay)
                {
                    m_defaultPackage.UnloadUnusedAssets();
                    m_unloadDelayCounter = 0f;
                }
            }
        }

        public async UniTask Predownload()
        {
            
#if UNITY_EDITOR
            //编辑器模式
            var initParameters = new EditorSimulateModeParameters();
            initParameters.SimulateManifestFilePath =
                EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
            await m_defaultPackage.InitializeAsync(initParameters);
#else
            //单机模式
            var initParameters = new OfflinePlayModeParameters();
            await m_defaultPackage.InitializeAsync(initParameters);
            
            //TODO:联机模式
#endif
            
        }

        public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken = default)
            where T : Object
        {
            var handle = this.m_defaultPackage.LoadAssetAsync<T>(assetName);
            await handle.Task;
            if (cancellationToken.IsCancellationRequested)
            {
                handle.Release();
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }

            this.m_assetHandleDict[handle.AssetObject] = handle;
            return handle.AssetObject as T;
        }

        public void ReleaseAsset<T>(T asset) where T : Object
        {
            if (this.m_assetHandleDict.TryGetValue(asset, out var handle))
            {
                handle.Release();
                this.m_assetHandleDict.Remove(asset);
            }
        }

        public async UniTask<GameObject> InstantiateAsync(string assetName, Transform parent = null,
            CancellationToken cancellationToken = default)
        {
            var handle = this.m_defaultPackage.LoadAssetAsync<GameObject>(assetName);
            await handle.Task;
            if (cancellationToken.IsCancellationRequested)
            {
                handle.Release();
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }

            var go = handle.InstantiateSync(parent);
            this.m_assetHandleDict[go] = handle;
            return go;
        }

        public void ReleaseInstance(GameObject go)
        {
            if (this.m_assetHandleDict.TryGetValue(go, out var handle))
            {
                Object.Destroy(go);
                handle.Release();
                this.m_assetHandleDict.Remove(go);
            }
        }

        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            CancellationToken cancellationToken = default)
        {
            var handle = this.m_defaultPackage.LoadSceneAsync(sceneName, mode, false);
            await handle;
            if (handle.IsMainScene())
                return;
            if (cancellationToken.IsCancellationRequested)
            {
                handle.UnloadAsync();
            }
            else
            {
                this.m_sceneHandleDict[sceneName] = handle;
            }
        }

        public async UniTask UnloadSceneAsync(string sceneName, UnloadSceneOptions unloadSceneOptions,
            CancellationToken cancellationToken = default)
        {
            if (this.m_sceneHandleDict.TryGetValue(sceneName, out var handle))
            {
                await handle.UnloadAsync();
                this.m_sceneHandleDict.Remove(sceneName);
            }
        }
    }
}