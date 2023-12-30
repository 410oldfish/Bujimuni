using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Lighten.Editor
{
    public static class TUIAtlasMakder
    {
        /*
         * 1.收集UIPrefab中引用到的图片资源
         * 2.根据图片资源所属Prefab分组
         * 3.根据分组生成SpriteAtlas
         * 4.设置SpriteAtlas的压缩格式
         */

        #region 定义类型

        public class SpriteData
        {
            public Sprite sprite;
            public string spritePath;
            public GameObject prefab;
            public string prefabPath;
        }

        public class SpriteGroupNode
        {
            public string groupName;
            public string assetPath;
            public List<Sprite> sprites = new List<Sprite>();
            public List<SpriteGroupNode> children = new List<SpriteGroupNode>();

            public static SpriteGroupNode Create(string groupName, string assetPath)
            {
                var node = new SpriteGroupNode();
                node.groupName = groupName;
                node.assetPath = assetPath;
                return node;
            }

            public string ToString(string spaceStr = "")
            {
                var content = $"{spaceStr}{groupName}";
                foreach (var child in children)
                {
                    content += $"\n{child.ToString(spaceStr + "    ")}";
                }
                return content;
            }
        }

        #endregion
        
        //
        public static void GenerateSpriteAtlasOfUI()
        {
            var setting = AssetDatabaseExtension.GetOrCreateScriptableObject<UIAtlasMakerSetting>(UIAtlasMakerSetting.DEFAULT_PATH);
            var folderPath = UIAtlasMakerSetting.ATLAS_PREFAB_PATH;
            
            //收集UIPrefab中引用到的图片资源
            var spritesMap = GetSpriteMap(folderPath);
            ExportSpriteMap(spritesMap, $"{Application.dataPath}/../SpriteMap.txt");
            //根据图片资源所属Prefab分组
            var root = CreateSpriteGroupTree(setting.spriteAtlasGroups);
            ExportSpriteGroupTree(root, $"{Application.dataPath}/../SpriteGroupTree.txt");
            var ignoreFolders = AssetDatabaseExtension.GetAssetPaths(setting.ignoreTextureFolders);
            UpdateSpriteGroupTree(root, spritesMap, ignoreFolders);
            
            //根据分组生成SpriteAtlas
            if (Directory.Exists(UIAtlasMakerSetting.ATLAS_OUTPUT_PATH))
            {
                Directory.Delete(UIAtlasMakerSetting.ATLAS_OUTPUT_PATH, true);
            }
            CreateSpriteAtlas(root, UIAtlasMakerSetting.ATLAS_OUTPUT_PATH);
            AssetDatabase.Refresh();
            
            //设置SpriteAtlas的压缩格式
            UpdateSpriteAtlasSettingByFolder(UIAtlasMakerSetting.ATLAS_OUTPUT_PATH);
            AssetDatabase.SaveAssets();
            
            //更新图集的预览
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
        }

        private static void CreateSpriteAtlas(SpriteGroupNode node, string outputDir)
        {
            if (node.sprites.Count > 0)
            {
                var assetPath = $"{outputDir}/{node.groupName}.spriteatlas";
                AssetDatabaseExtension.CreateSpriteAtlas(assetPath, node.sprites);
            }
            foreach (var child in node.children)
            {
               CreateSpriteAtlas(child, outputDir); 
            }
        }
        
        public static void ExportSpriteMap(Dictionary<Sprite, List<GameObject>> spriteMap, string outputPath)
        {
            var content = string.Empty;
            var list = new List<(Sprite, List<GameObject>)>();
            foreach (var pairs in spriteMap)
            {
                list.Add((pairs.Key, pairs.Value));
            }

            list.Sort((a, b) => { return String.Compare(a.Item1.name, b.Item1.name, StringComparison.Ordinal); });
            foreach (var item in list)
            {
                content += $"{item.Item1.name}:\n";
                foreach (var prefab in item.Item2)
                {
                    content += $"    {prefab.name}\n";
                }
            }
            DirectoryExtension.GenerateDirectory(outputPath);
            File.WriteAllText(outputPath, content);
        }
        
        public static void ExportSpriteGroupTree(SpriteGroupNode root, string outputPath)
        {
            var content = root.ToString();
            DirectoryExtension.GenerateDirectory(outputPath);
            File.WriteAllText(outputPath, content);
        }
        
        //构建SpriteGroup树
        private static SpriteGroupNode CreateSpriteGroupTree(List<Object> spriteAtlasGroups)
        {
            var root = SpriteGroupNode.Create("AutoCommon", string.Empty);
            foreach (var spriteAtlasGroup in spriteAtlasGroups)
            {
                var assetPath = AssetDatabase.GetAssetPath(spriteAtlasGroup);
                if (string.IsNullOrEmpty(assetPath))
                    continue;
                AddSpriteGroupNode(root, assetPath);
            }
            return root;
        }

        private static void AddSpriteGroupNode(SpriteGroupNode node, string assetPath)
        {
            var groupName = Path.GetFileNameWithoutExtension(assetPath);
            if (node.children.Count < 1)
            {
                node.children.Add(SpriteGroupNode.Create(groupName, assetPath));
                return;
            }
            foreach (var child in node.children)
            {
                if (assetPath.Contains(child.assetPath))
                {
                    AddSpriteGroupNode(child, assetPath);
                    return;
                }
            }
            //新建节点
            SpriteGroupNode newNode = SpriteGroupNode.Create(groupName, assetPath);
            //看看有没有子节点是新节点的子节点
            foreach (var child in node.children)
            {
                if (child.assetPath.Contains(assetPath))
                {
                    newNode.children.Add(child);
                }
            }
            foreach (var child in newNode.children)
            {
                node.children.Remove(child);
            }
            node.children.Add(newNode);
        }
        
        //根据sprite映射表分组
        private static void UpdateSpriteGroupTree(SpriteGroupNode root, Dictionary<Sprite, List<GameObject>> spritesMap, IEnumerable<string> ingoreFolders = null)
        {
            foreach (var pairs in spritesMap)
            {
                var sprite = pairs.Key;
                var assetPath = AssetDatabase.GetAssetPath(sprite);
                if (string.IsNullOrEmpty(assetPath))
                    continue;
                if (ingoreFolders != null && IsFileInFolders(assetPath, ingoreFolders))
                    continue;
                UpdateSpriteGroupNode(root, sprite, pairs.Value);
            }
        }
        private static void UpdateSpriteGroupNode(SpriteGroupNode node, Sprite sprite, List<GameObject> prefabs)
        {
            int num = 0;
            SpriteGroupNode containNode = null;
            foreach (var prefab in prefabs)
            {
                var assetPath = AssetDatabase.GetAssetPath(prefab);
                foreach (var child in node.children)
                {
                    if (assetPath.Contains(child.assetPath))
                    {
                        num++;
                        containNode = child;
                    }
                }
            }
            if (num == 1)
            {
                UpdateSpriteGroupNode(containNode, sprite, prefabs);
                return;
            }
            node.sprites.Add(sprite);
        }

        //获取所有sprite和prefab的映射关系
        private static Dictionary<Sprite, List<GameObject>> GetSpriteMap(string folderPath)
        {
            var result = new Dictionary<Sprite, List<GameObject>>();
            var filePaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                if (prefab == null)
                    continue;
                var sprites = GetSprites(prefab);
                if (sprites == null || sprites.Count < 1)
                    continue;
                foreach (var sprite in sprites)
                {
                    if (result.ContainsKey(sprite))
                    {
                        result[sprite].Add(prefab);
                    }
                    else
                    {
                        result.Add(sprite, new List<GameObject>() { prefab });
                    }
                }
            }

            return result;
        }
        
        //获取prefab上的所有sprite,不重复
        private static List<Sprite> GetSprites(GameObject prefab)
        {
            if (prefab == null)
                return null;
            var sprites = new List<Sprite>();
            var images = prefab.GetComponentsInChildren<Image>(true);
            foreach (var image in images)
            {
                if (image.sprite == null)
                    continue;
                var sprite = image.sprite;
                if (sprites.Contains(sprite))
                    continue;
                var spriteAssetPath = AssetDatabase.GetAssetPath(sprite);
                if (spriteAssetPath == "Resources/unity_builtin_extra")
                    continue;
                sprites.Add(sprite);
            }

            return sprites;
        }
        
        //检测文件是否在指定文件夹列表中
        private static bool IsFileInFolders(string filePath, IEnumerable<string> folders)
        {
            foreach (var folder in folders)
            {
                if (filePath.Contains(folder))
                    return true;
            }
            return false;
        }

        //更新图集设置
        public static void UpdateSpriteAtlasSettingByFolder(string folderPath)
        {
            var filePaths = Directory.GetFiles(folderPath, "*.spriteatlas", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                UpdateSpriteAtlasSetting(filePath);
            }
        }
        //更新图集设置
        public static void UpdateSpriteAtlasSetting(string assetPath)
        {
            //TODO: 暂时不设置格式
            Debug.LogWarning("TODO:暂时不自动设置格式,等格式插件接入");
            return;
            // var textureFormatSchema = TextureFormatHelper.GetCacheTextureFormatSchema();
            // if (textureFormatSchema == null)
            //     return;
            // var settingsName = TextureFormatHelper.GetTextureSettingsName(EditorUserBuildSettings.activeBuildTarget);
            // if (settingsName == EnumTexturesSettingName.None)
            //     return;
            // var textureFormat = textureFormatSchema.GetTextureFormat(settingsName);
            // if (textureFormat == null)
            //     return;
            // if (!TextureFormatHelper.IsAllowedInPostprocessor(assetPath))
            //     return;
            //Debug.Log($"OnPreprocessAsset {assetPath}");
            // var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            // if (spriteAtlas == null)
            //     return;
            // //Debug.Log($"OnPreprocessAsset {assetPath} success");
            // //var settings = spriteAtlas.GetPlatformSettings(settingsName.ToString());
            // //settings.format = textureFormat.format;
            // //settings.overridden = true;
            // //spriteAtlas.SetPlatformSettings(settings);
            // EditorUtility.SetDirty(spriteAtlas);
        }
    }
}