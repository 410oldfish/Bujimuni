using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Lighten.Editor
{
    public static class AssetDatabaseExtension
    {
        /// <summary>
        /// 获取ScriptableObject,如果没有,就创建一个并返回
        /// </summary>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrCreateScriptableObject<T>(string filePath) where T : ScriptableObject
        {
            var scriptableObject = AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (scriptableObject != null)
            {
                return scriptableObject;
            }
            scriptableObject = ScriptableObject.CreateInstance<T>();
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            AssetDatabase.CreateAsset(scriptableObject, filePath);
            return scriptableObject;
        }

        //创建SpriteAtlas
        public static void CreateSpriteAtlas(string atlasPath, List<Sprite> sprites)
        {
            var atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);
            if (atlas != null)
            {
                //每次都重新生成图集资源,因为未知原因,已存在的图集无法读取已有的数据
                AssetDatabase.DeleteAsset(atlasPath);
            }
            atlas = new SpriteAtlas();
            var packingSetting = new SpriteAtlasPackingSettings();
            packingSetting.blockOffset = 1;
            packingSetting.enableRotation = false;
            packingSetting.enableTightPacking = false;
            packingSetting.padding = 2;
            atlas.SetPackingSettings(packingSetting);

            var textureSetting = new SpriteAtlasTextureSettings();
            textureSetting.readable = false;
            textureSetting.generateMipMaps = false;
            textureSetting.sRGB = false;
            textureSetting.filterMode = FilterMode.Bilinear;
            atlas.SetTextureSettings(textureSetting);

            atlas.Add(sprites.ToArray());

            var dir = Path.GetDirectoryName(atlasPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            AssetDatabase.CreateAsset(atlas, atlasPath);
        }

        /// <summary>
        /// 查找资源路径列表
        /// </summary>
        /// <param name="assetPaths">查找目标对象列表</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static List<string> SearchAssetPaths(string[] assetPaths, string pattern,
            System.Func<string, bool> filter = null)
        {
            var flags = new Dictionary<string, bool>();
            var result = new List<string>();
            if (assetPaths == null || assetPaths.Length < 1)
                return result;
            foreach (var assetPath in assetPaths)
            {
                //GameScene实例对象
                if (string.IsNullOrEmpty(assetPath))
                    continue;
                //如果是文件夹,则查找它下面的所有文件
                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    var filePaths = Directory.GetFiles(assetPath, pattern, SearchOption.AllDirectories);
                    foreach (var filePath in filePaths)
                    {
                        if (flags.ContainsKey(filePath))
                            continue;
                        if (filter == null || filter.Invoke(filePath))
                        {
                            result.Add(filePath);
                            flags.Add(filePath, true);
                        }
                    }
                }
                else
                {
                    if (flags.ContainsKey(assetPath))
                        continue;
                    int patternDotIndex = pattern.IndexOf(".");
                    if (patternDotIndex != -1)
                    {
                        var ext = pattern.Substring(patternDotIndex);
                        if (ext != ".*" && !assetPath.EndsWith(ext))
                            continue;
                    }
                    if (filter == null || filter.Invoke(assetPath))
                    {
                        result.Add(assetPath);
                        flags.Add(assetPath, true);
                    }
                }
            }
            return result;
        }
        
        //
        public static List<string> GetAssetPaths(List<Object> objects)
        {
            var result = new List<string>();
            foreach (var obj in objects)
            {
                if (obj == null)
                    continue;
                var assetPath = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(assetPath))
                    continue;
                result.Add(assetPath);
            }
            return result;
        }
        
        //获取对象来源prefab
        public static GameObject GetPrefabInstanceRoot(GameObject obj)
        {
            var prefabType = PrefabUtility.GetPrefabAssetType(obj);
            var assetPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(assetPath))
            {
                //如果是场景中的对象,则获取其prefab
                obj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            }
            //如果是变体,那么再获取其源prefab
            if (prefabType == PrefabAssetType.Variant)
            {
                return PrefabUtility.GetCorrespondingObjectFromSource(obj);
            }
            return obj;
        }
        
        public static GameObject GetSourceGameObject(GameObject gameObject)
        {
            var result = gameObject;
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(gameObject);
            switch (prefabAssetType)
            {
                case PrefabAssetType.Regular:
                case PrefabAssetType.Model:
                case PrefabAssetType.Variant:
                    if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
                    {
                        result = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
                    }
                    break;
                case PrefabAssetType.NotAPrefab:
                    break;
            }

            return result;
        }
        
        //根据prefab名字获取prefab
        public static GameObject LoadPrefabByName(string prefabName, string searchInFolder = null)
        {
            if (string.IsNullOrEmpty(searchInFolder))
            {
                searchInFolder = "Assets";
            }
            string[] guids = AssetDatabase.FindAssets($"{prefabName} t:Prefab", new[] { searchInFolder });
            if (guids.Length < 1)
                return null;
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null && prefab.name == prefabName)
                    return prefab;
            }
            return null;
        }
        
        //查找工程目录中离自己最近的文件夹
        public static string SearchFolderInParent(string assetPath, string folderName)
        {
            var assetDir = Path.GetDirectoryName(assetPath);
            if (string.IsNullOrEmpty(assetDir))
            {
                return string.Empty;
            }
            assetDir = assetDir.Replace("/", "\\");
            var assetDirArray = assetDir.Split('\\');
            var endStr = "\\" + folderName;
            for (int i = assetDirArray.Length - 1; i >= 0; i--)
            {
                var folderPath = string.Join("\\", assetDirArray, 0, i + 1);
                Debug.Log($"check folderPath:{folderPath}");
                var directories = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);
                if (directories.Length < 1)
                    continue;
                foreach (var dir in directories)
                {
                    if (dir.EndsWith(endStr))
                    {
                        return dir.Replace("\\", "/");
                    }
                }
            }

            return string.Empty;
        }
    }
}