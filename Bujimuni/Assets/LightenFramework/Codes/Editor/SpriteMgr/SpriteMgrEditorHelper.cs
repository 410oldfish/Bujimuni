using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace Lighten.Editor
{
    public static class SpriteMgrEditorHelper
    {
        public static bool GenerateSpriteDataConfig()
        {
            var spriteMgrEditorSetting =
                    AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteMgrEditorSetting>(SpriteMgrEditorSetting.DEFAULT_PATH);
            return GenerateSpriteDataConfig(spriteMgrEditorSetting);
        }

        public static bool GenerateSpriteDataConfig(SpriteMgrEditorSetting spriteMgrEditorSetting)
        {
            if (string.IsNullOrEmpty(spriteMgrEditorSetting.outputFolder))
            {
                Debug.LogError("请先配置输出目录");
                return false;
            }

            var folderPaths = AssetDatabaseExtension.GetAssetPaths(spriteMgrEditorSetting.folders);
            if (folderPaths.Count < 1)
            {
                Debug.LogError("请先配置查询目录");
                return false;
            }
            var spriteDataMap = new Dictionary<string, SpriteMgrConfig.SpriteData>();
            CollectSpriteDataBySpriteAtlas(ref spriteDataMap, folderPaths);
            CollectSpriteDataBySprite(ref spriteDataMap, folderPaths);

            //检查一下重复命名
            if (CheckDuplicateName(spriteDataMap))
                return false;
            
            var configPath = $"{spriteMgrEditorSetting.outputFolder}/{SpriteMgrConfig.DEFAULT_NAME}.asset";
            var spriteMgrConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteMgrConfig>(configPath);
            spriteMgrConfig.Datas.Clear();
            foreach (var data in spriteDataMap.Values)
            {
                spriteMgrConfig.Datas.Add(data);
            }
            EditorUtility.SetDirty(spriteMgrConfig);
            AssetDatabase.SaveAssetIfDirty(spriteMgrConfig);
            return true;
        }

        private static void CollectSpriteDataBySpriteAtlas(ref Dictionary<string, SpriteMgrConfig.SpriteData> spriteDataMap, List<string> folderPaths)
        {
            var assetPaths = AssetDatabaseExtension.SearchAssetPaths(folderPaths.ToArray(), "*.spriteatlas");
            foreach (var assetPath in assetPaths)
            {
                var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
                var assetName = spriteAtlas.name;
                if (spriteDataMap.ContainsKey(assetName))
                    continue;
                var spriteData = new SpriteMgrConfig.SpriteData();
                spriteData.Type = ESpriteType.SpriteAtlas;
                spriteData.AssetName = assetName;
                
                var sprites = new Sprite[spriteAtlas.spriteCount];
                var count = spriteAtlas.GetSprites(sprites);
                for (int i = 0; i < count; ++i)
                {
                    var sprite = sprites[i];
                    spriteData.SpriteNames.Add(ConvertSpriteName(sprite.name));
                }
                spriteDataMap.Add(assetName, spriteData);
            }
        }
        
        private static void CollectSpriteDataBySprite(ref Dictionary<string, SpriteMgrConfig.SpriteData> spriteDataMap, List<string> folderPaths)
        {
            var assetPaths = AssetDatabaseExtension.SearchAssetPaths(folderPaths.ToArray(), "*.png");
            foreach (var assetPath in assetPaths)
            {
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                var assetName = sprite.texture.name;
                if (spriteDataMap.ContainsKey(assetName))
                    continue;
                var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
                if (subAssets.Length < 1)
                    continue;
                var spriteData = new SpriteMgrConfig.SpriteData();
                spriteData.AssetName = assetName;
                //spritesheet第一个是texture,其余是sprite
                if (subAssets.Length > 2)
                {
                    spriteData.Type = ESpriteType.SpriteSheet;
                    foreach (var subAsset in subAssets)
                    {
                        var spriteName = ConvertSpriteName(subAsset.name);
                        if (spriteName == assetName)
                            continue;
                        spriteData.SpriteNames.Add(spriteName);
                    }
                }
                else
                {
                    spriteData.Type = ESpriteType.Sprite;
                    spriteData.SpriteNames.Add(sprite.name);
                }
                spriteDataMap.Add(assetName, spriteData);
            }
        }

        private static bool CheckDuplicateName(Dictionary<string, SpriteMgrConfig.SpriteData> spriteDataMap)
        {
            var spriteNameMap = new Dictionary<string, bool>();
            var hasDuplicateName = false;
            foreach (var spriteData in spriteDataMap.Values)
            {
                foreach (var spriteName in spriteData.SpriteNames)
                {
                    if (spriteNameMap.ContainsKey(spriteName))
                    {
                        Debug.LogError($"Sprite 存在重复命名 {spriteName}");
                        hasDuplicateName = true;
                        continue;
                    }
                    spriteNameMap.Add(spriteName, true);
                }
            }
            return hasDuplicateName;
        }
        
        private static string ConvertSpriteName(string spriteName)
        {
            if (spriteName.EndsWith("(Clone)"))
            {
                return spriteName.Replace("(Clone)", string.Empty);
            }

            return spriteName;
        }
    }
}