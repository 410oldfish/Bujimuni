using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;

namespace Lighten
{
    public class SpriteLoader : XEntity, IAwake, IDestroy
    {
        private SpriteMgr m_spriteMgr;
        private ResLoader m_resLoader;

        public void Awake()
        {
            this.m_spriteMgr = Game.Architecture.GetManager<SpriteMgr>();
            this.m_resLoader = this.AddComponent<ResLoader>();
        }

        public void Destroy()
        {
        }

        public async UniTask<Sprite> LoadSpriteAsync(string spriteName, CancellationToken cancellationToken = default)
        {
            var spriteData = await this.m_spriteMgr.GetSpriteData(spriteName);
            if (spriteData == null)
            {
                Debug.LogError($"sprite {spriteName} 不存在");
                return null;
            }

            Sprite result = null;

            switch (spriteData.SpriteType)
            {
                case ESpriteType.Sprite:
                    result = await this.m_resLoader.LoadAssetAsync<Sprite>(spriteName, cancellationToken);
                    break;
                case ESpriteType.SpriteAtlas:
                {
                    var spriteAtlas =
                        await this.m_resLoader.LoadAssetAsync<SpriteAtlas>(spriteData.AssetName, cancellationToken);
                    if (spriteAtlas == null)
                        return null;
                    result = spriteAtlas.GetSprite(spriteName);
                }
                    break;
                case ESpriteType.SpriteSheet:
                    result = await this.m_resLoader.LoadAssetAsync<Sprite>($"{spriteData.AssetName}[{spriteName}]",
                        cancellationToken);
                    break;
            }

            return result;
        }
    }
}