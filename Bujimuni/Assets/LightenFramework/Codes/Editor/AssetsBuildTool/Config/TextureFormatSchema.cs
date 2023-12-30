#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lighten.Editor
{
    /*
         * https://docs.unity3d.com/cn/2020.2/ScriptReference/TextureImporter.GetPlatformTextureSettings.html
         * The values for the chosen platform are returned in the "out" parameters.
         * The options for the platform string are "Standalone", "Web", "iPhone", "Android", "WebGL", "Windows Store Apps", "PS4", "XboxOne", "Nintendo Switch" and "tvOS".
        */
    public enum EnumTexturesSettingName
    {
        None = 0,
        Standalone,
        iPhone,
        Android,
        WebGL,
    }
    public class TextureFormatSchema : ScriptableObject
    {
        [Serializable]
        public class TextureFormatData
        {
            public EnumTexturesSettingName settingName;
            public TextureImporterFormat format;
        }

        [Serializable]
        public class TextureFilterData
        {
            [LabelText("扩展名")]
            public string extensionName = ".png";
            [LabelText("检索目录")]
            public List<Object> folders = new List<Object>();
        }

        [LabelText("图片压缩格式")] public List<TextureFormatData> textureFormatDatas = new List<TextureFormatData>();
        [LabelText("自动处理范围")] public List<TextureFilterData> textureFilterDatas = new List<TextureFilterData>();

        [Button("清 理 缓 存", ButtonSizes.Large), GUIColor(0, 1, 0)]
        public void ClearCache()
        {
            EditorUtility.RequestScriptReload();
        }

        public TextureFormatData GetTextureFormat(EnumTexturesSettingName settingName)
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            foreach (var textureFormat in textureFormatDatas)
            {
                if (textureFormat.settingName == settingName)
                    return textureFormat;
            }
            return null;
        }
    }
}

#endif