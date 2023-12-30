using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten.Editor
{
    public static class AssetsBuildToolsDefine
    {
        //图片格式方案路径
        public static string TEXTURE_FORMAT_SCHEMA_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/EditorConfig/TextureFormatSchema.asset";
        //Addressable方案路径
        public static string ADDRESSABLE_SETUP_SCHEMA_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/EditorConfig/AddressableSetupSchema.asset";
        
        //UI 图集方案路径
        public static string UI_SPRITE_ATLAS_SETUP_SCHEMA_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/EditorConfig/UISpriteAtlasSetupSchema.asset";
        //UI 资源数据名
        public static string UI_SPRITE_ATLAS_DATA_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/EditorConfig/UISpriteAtlasData.asset";
        //UI prefab路径
        public static string UI_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/Bundles/Prefabs/UI/";
        //UI atlas路径
        public static string UI_ATLAS_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/Bundles/Atlas/UI/";
        //
        public static string ATLAS_COMMON = "Common";

    }
}
