using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Lighten.Editor
{
    public class TextureFormatPostprocessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            var textureFormatSchema = TextureFormatHelper.GetTextureFormatSchema();
            if (textureFormatSchema == null)
                return;
            var settingsName = TextureFormatHelper.GetTextureSettingsName(EditorUserBuildSettings.activeBuildTarget);
            if (settingsName == EnumTexturesSettingName.None)
                return;
            var textureFormat = textureFormatSchema.GetTextureFormat(settingsName);
            if (textureFormat == null)
                return;
            if (!TextureFormatHelper.IsAllowedInPostprocessor(assetPath))
                return;
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
                return;
            //Debug.Log(assetPath);
            var settings = importer.GetPlatformTextureSettings(settingsName.ToString());
            settings.format = textureFormat.format;
            settings.overridden = true;
            importer.SetPlatformTextureSettings(settings);
        }

        // private void OnPreprocessAsset()
        // {
        //     
        // }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                var extensionName = Path.GetExtension(importedAsset);
                if (extensionName == ".spriteatlas")
                {
                    OnPostprocessSpriteAtlas(importedAsset);
                }
            }
        }

        private static void OnPostprocessSpriteAtlas(string assetPath)
        {
            var textureFormatSchema = TextureFormatHelper.GetTextureFormatSchema();
            if (textureFormatSchema == null)
                return;
            var settingsName = TextureFormatHelper.GetTextureSettingsName(EditorUserBuildSettings.activeBuildTarget);
            if (settingsName == EnumTexturesSettingName.None)
                return;
            var textureFormat = textureFormatSchema.GetTextureFormat(settingsName);
            if (textureFormat == null)
                return;
            if (!TextureFormatHelper.IsAllowedInPostprocessor(assetPath))
                return;
            //Debug.Log($"OnPreprocessAsset {assetPath}");
            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(assetPath);
            if (spriteAtlas == null)
                return;
            //Debug.Log($"OnPreprocessAsset {assetPath} success");
            var settings = spriteAtlas.GetPlatformSettings(settingsName.ToString());
            settings.format = textureFormat.format;
            settings.overridden = true;
            spriteAtlas.SetPlatformSettings(settings);
        }
    }
}