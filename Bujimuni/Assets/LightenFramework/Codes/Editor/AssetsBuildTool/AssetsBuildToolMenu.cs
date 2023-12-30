using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace Lighten.Editor
{
    public static class AssetsBuildToolMenu
    {
        // [MenuItem("LightenFramework/资 源 管 理")]
        // static void OpenWindow()
        // {
        //     var window = EditorWindow.GetWindow<AssetsBuildToolWindow>("资 源 管 理");
        //     window.Show();
        // }

        [MenuItem("Assets/LightenFramework/查 找 界 面 引 用", priority = -50)]
        static void SearchReference()
        {
            Sprite sprite = null;
            if (Selection.activeObject is Texture)
            {
                var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            }

            if (Selection.activeObject is Sprite)
            {
                sprite = Selection.activeObject as Sprite;
            }

            if (sprite == null)
            {
                EditorUtility.DisplayDialog("警告", "必须选中一张sprite图片", "OK");
                return;
            }

            //SpriteAtlasHelper.SearchReferenceUI(sprite);
        }

        [MenuItem("Assets/LightenFramework/查 找 默 认 字 体 引 用", priority = -50)]
        static void SearchTMPDefaultFontReference()
        {
            if (Selection.activeObject == null)
                return;
            var folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!AssetDatabase.IsValidFolder(folderPath))
                return;
            var tmpDefaultFonts = new List<TMP_FontAsset>();
            tmpDefaultFonts.Add(
                AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                    "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset"));
            tmpDefaultFonts.Add(AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset"));

            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folderPath });
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                    continue;
                var tmpMonos = asset.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var tmpMono in tmpMonos)
                {
                    if (tmpMono.font != null && tmpDefaultFonts.Contains(tmpMono.font))
                    {
                        Debug.LogError($"use tmp default font {assetPath}\n{asset.transform.GetChildPath(tmpMono.transform)}");
                    }
                }
            }
        }
    }
}