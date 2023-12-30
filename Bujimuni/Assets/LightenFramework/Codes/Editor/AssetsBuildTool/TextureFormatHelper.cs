using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lighten.Editor
{
    public static class TextureFormatHelper
    {
        public static TextureFormatSchema GetOrCreateTextureFormatSchema()
        {
            return AssetDatabaseExtension.GetOrCreateScriptableObject<TextureFormatSchema>(AssetsBuildToolsDefine.TEXTURE_FORMAT_SCHEMA_PATH);
        }
        public static TextureFormatSchema GetTextureFormatSchema()
        {
            return AssetDatabase.LoadAssetAtPath<TextureFormatSchema>(AssetsBuildToolsDefine.TEXTURE_FORMAT_SCHEMA_PATH);
        }

        private static Dictionary<BuildTarget, EnumTexturesSettingName> m_buildTargetToPlatformName
            = new Dictionary<BuildTarget, EnumTexturesSettingName>()
            {
                {BuildTarget.StandaloneWindows, EnumTexturesSettingName.Standalone},
                {BuildTarget.StandaloneWindows64, EnumTexturesSettingName.Standalone},
                {BuildTarget.Android, EnumTexturesSettingName.Android},
                {BuildTarget.iOS, EnumTexturesSettingName.iPhone},
                {BuildTarget.WebGL, EnumTexturesSettingName.WebGL},
            };

        public static EnumTexturesSettingName GetTextureSettingsName(BuildTarget buildTarget)
        {
            if (m_buildTargetToPlatformName.ContainsKey(buildTarget))
                return m_buildTargetToPlatformName[buildTarget];
            return EnumTexturesSettingName.None;
        }

        private static TextureFormatSchema m_cacheSchema;
        public static TextureFormatSchema GetCacheTextureFormatSchema()
        {
            if (m_cacheSchema == null)
            {
                m_cacheSchema = GetOrCreateTextureFormatSchema();
            }
            return m_cacheSchema;
        }
        public static bool IsAllowedInPostprocessor(string assetPath)
        {
            if (m_cacheSchema == null || string.IsNullOrEmpty(assetPath))
                return false;
            foreach (var filterData in m_cacheSchema.textureFilterDatas)
            {
                if (filterData.folders == null)
                    continue;
                foreach (var folderObj in filterData.folders)
                {
                    var targetPath = AssetDatabase.GetAssetPath(folderObj);
                    if (!AssetDatabase.IsValidFolder(targetPath))
                        continue;
                    var extensionName = Path.GetExtension(assetPath);
                    if (assetPath.Contains(targetPath) && extensionName == filterData.extensionName)
                        return true;
                }
            }
            return false;
        }
    }
}