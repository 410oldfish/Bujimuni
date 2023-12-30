using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighten
{
    public class SpriteData
    {
        public string SpriteName;
        public ESpriteType SpriteType;
        public string AssetName;
    }
    
    public interface ISpriteMgr
    {
        UniTask<SpriteData> GetSpriteData(string spriteName);
    }

    public class SpriteMgr : AbstractManager, ISpriteMgr, IAwake, IDestroy
    {
        private bool m_initialized = false;
        private Dictionary<string, SpriteData> m_spriteDataDict = new Dictionary<string, SpriteData>();

        public void Awake()
        {
            this.m_initialized = false;
        }

        public void Destroy()
        {
        }
        
        public async UniTask<SpriteData> GetSpriteData(string spriteName)
        {
            if (!this.m_initialized)
            {
                await this.LoadConfig();
            }

            if (this.m_spriteDataDict.TryGetValue(spriteName, out var spriteData))
            {
                return spriteData;
            }

            return null;
        }

        private async UniTask LoadConfig()
        {
            var config = await this.GetManager<IResMgr>().LoadAssetAsync<SpriteMgrConfig>(SpriteMgrConfig.DEFAULT_NAME);
            if (config == null)
            {
                Debug.LogError("SpriteDataConfig不存在");
                return;
            }

            this.m_spriteDataDict.Clear();
            foreach (var data in config.Datas)
            {
                foreach (var spriteName in data.SpriteNames)
                {
                    if (this.m_spriteDataDict.ContainsKey(spriteName))
                    {
                        Debug.LogError($"重复sprite {spriteName} {data.Type} {data.AssetName}");
                        continue;
                    }

                    var spriteData = new SpriteData()
                    {
                        SpriteName = spriteName,
                        SpriteType = data.Type,
                        AssetName = data.AssetName,
                    };
                    this.m_spriteDataDict.Add(spriteName, spriteData);
                }
            }

            this.GetManager<IResMgr>().ReleaseAsset(config);
            this.m_initialized = true;
        }
    }
}