using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighten
{
    public class ResLoader : XEntity, IAwake, IDestroy
    {
        private IResMgr m_resMgr;

        private Dictionary<string, Object> m_cachedAssets = new Dictionary<string, Object>();
        private Dictionary<string, bool> m_loadingStates = new Dictionary<string, bool>();
        private List<GameObject> m_cachedGameObjects = new List<GameObject>();

        public void Awake()
        {
            m_resMgr = Game.Architecture.GetManager<IResMgr>();
        }

        public void Destroy()
        {
            if (this.m_cachedAssets.Count > 0)
            {
                foreach (var kv in this.m_cachedAssets)
                {
                    m_resMgr.ReleaseAsset(kv.Value);
                }

                this.m_cachedAssets.Clear();
            }

            if (this.m_cachedGameObjects.Count > 0)
            {
                foreach (var gameObject in this.m_cachedGameObjects)
                {
                    m_resMgr.ReleaseInstance(gameObject);
                }

                this.m_cachedGameObjects.Clear();
            }
        }

        //加载资源
        public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken = default)
            where T : Object
        {
            while (this.m_loadingStates.ContainsKey(assetName))
            {
                await UniTask.DelayFrame(1);
            }

            if (this.m_cachedAssets.ContainsKey(assetName))
            {
                return this.m_cachedAssets[assetName] as T;
            }

            this.m_loadingStates[assetName] = true;
            //这里必须自己捕获异常,保证CachedAssetsLoading.Remove能执行到,然后再次抛出取消异常
            var result = await m_resMgr.LoadAssetAsync<T>(assetName, cancellationToken).SuppressCancellationThrow();
            this.m_loadingStates.Remove(assetName);
            if (result.IsCanceled)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }

            var asset = result.Result;
            if (asset == null)
            {
                return null;
            }

            this.m_cachedAssets[assetName] = asset;
            return asset;
        }
        
        //卸载资源
        public void ReleaseAsset(string assetName)
        {
            if (!this.m_cachedAssets.TryGetValue(assetName, out var asset))
            {
                return;
            }

            this.m_cachedAssets.Remove(assetName);
            m_resMgr.ReleaseAsset(asset);
        }
        
        //实例化GameObject
        public async UniTask<GameObject> InstantiateAsync(string assetName, Transform parent = null, CancellationToken cancellationToken = default)
        {
            var go = await this.m_resMgr.InstantiateAsync(assetName, parent, cancellationToken);
            if (go == null)
            {
                return null;
            }
            this.m_cachedGameObjects.Add(go);
            return go;
        }
        
        //销毁并释放GameObject
        public void ReleaseInstance(GameObject go)
        {
            if (!this.m_cachedGameObjects.Contains(go))
            {
                return;
            }

            this.m_cachedGameObjects.Remove(go);
            m_resMgr.ReleaseInstance(go);
        }
    }
}