using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Lighten
{
    public static class SpriteExtentions
    {
        #region Image扩展

        public static void SetSprite(this UIEntity uiEntity, Image image, string spriteName)
        {
            uiEntity.SetSpriteAsync(image, spriteName).Forget();
        }

        public static async UniTask SetSpriteAsync(this UIEntity uiEntity, Image image, string spriteName)
        {
            image.enabled = false;
            image.sprite = await uiEntity.SpriteLoader.LoadSpriteAsync(spriteName, uiEntity.UniTaskCTS.Default);
            image.enabled = true;
        }

        #endregion
    }
}