using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;

namespace Lighten.Editor
{
    public class AssetsBuildToolWindow : OdinEditorWindow
    {
#if ENABLE_CODES
        [Button("当前模式:本地代码模式", ButtonSizes.Gigantic), GUIColor(0.0f, 1.0f, 0.0f)]
        void VirtualButton()
        {
            HybridCLRTool.EnableCodes(false);
        }
#else
        [Button("当前模式:打包代码模式", ButtonSizes.Gigantic), GUIColor(1.0f, 0.0f, 1.0f)]
        void VirtualButton()
        {
            //HybridCLRTool.EnableCodes(true);
        }
#endif


        [HorizontalGroup("1")]
        [Button("Addressable", ButtonSizes.Large)]
        void OpenAddressableScheme()
        {
            //TODO: 这里改用AddressableImporter插件
            // var addressableSetupSchema =
            //     AssetDatabaseExtension.GetOrCreateScriptableObject<AddressableSetupSchema>(
            //         AssetsBuildToolsDefine.ADDRESSABLE_SETUP_SCHEMA_PATH);
            // Selection.activeObject = addressableSetupSchema;
            // var settingList = AddressableImportSettingsList.Instance.SettingList;
            // if (settingList.Count < 1)
            //     return;
            // Selection.activeObject = settingList[0];
        }

        [HorizontalGroup("1")]
        [Button("精灵配置", ButtonSizes.Large)]
        static void OpenSpriteDataScheme()
        {
            // var spriteDataSchema =
            //     AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteDataSchema>(
            //         SpriteManagerToolDefine.SPRITE_DATA_SCHEMA_PATH);
            // Selection.activeObject = spriteDataSchema;
        }

        [HorizontalGroup("1")]
        [Button("UI图集策略", ButtonSizes.Large)]
        void OpenSpriteAtlasScheme()
        {
            var spriteAtlasSetupSchema =
                AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteAtlasSetupSchema>(
                    AssetsBuildToolsDefine.UI_SPRITE_ATLAS_SETUP_SCHEMA_PATH);
            Selection.activeObject = spriteAtlasSetupSchema;
        }

        [HorizontalGroup("1")]
        [Button("图片压缩策略", ButtonSizes.Large)]
        void OpenTextureFormatScheme()
        {
            var textureFormatSchema =
                AssetDatabaseExtension.GetOrCreateScriptableObject<TextureFormatSchema>(
                    AssetsBuildToolsDefine.TEXTURE_FORMAT_SCHEMA_PATH);
            Selection.activeObject = textureFormatSchema;
        }

        [HorizontalGroup("1")]
        [Button("HybridCLR", ButtonSizes.Large)]
        void OpenHybridCLRScheme()
        {
            //HybridCLRTool.EditHybridCLRAssemblyNames();
        }

        //---------------------------------------------------------------------------------------------------
        [BoxGroup("更 新 配 置", order: 2)]
        [Button("更 新 Sprite 配 置", ButtonSizes.Medium), GUIColor(1.0f, 1.0f, 0.0f)]
        void UpdateSpriteData()
        {
            // SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
            // var success = SpriteManagerToolHelper.GenerateSpriteDataConfig();
            // if (success)
            // {
            //     EditorUtility.DisplayDialog("提示", "生成成功!", "OK");
            //     Selection.activeObject =
            //         AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteDataConfig>(
            //             SpriteManagerToolDefine.SPRITE_DATA_CONFIG_PATH);
            // }
            // else
            // {
            //     EditorUtility.DisplayDialog("提示", "失败!请查看控制台信息", "OK");
            // }
        }

        [BoxGroup("更 新 配 置")]
        [Button("更 新 UI 图 集", ButtonSizes.Medium), GUIColor(1.0f, 0.7f, 0.0f)]
        void UpdateAtlas()
        {
            //SpriteAtlasHelper.CreateAtlasData();
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
        }

        [BoxGroup("更 新 配 置")]
        [Button("更 新 Font 资 源", ButtonSizes.Medium), GUIColor(1.0f, 1.0f, 0.0f)]
        void UpdateFont()
        {
            TextCollector.Collect();
        }

        [BoxGroup("更 新 配 置")]
        [Button("更 新 Luban 配 置", ButtonSizes.Medium), GUIColor(1.0f, 0.7f, 0.0f)]
        void UpdateLubanData()
        {
            // var success = UpdateLubanDataInternal();
            // if (success)
            // {
            //     AddressableImporter.FolderImporter.ReimportFolders(new[] { "Assets/_GameClient/Raw/Configs" }, false);
            //     EditorUtility.DisplayDialog("提示", "luban配置生成成功!", "OK");
            // }
            // else
            // {
            //     EditorUtility.DisplayDialog("提示", "失败!没找到luban导出配置", "OK");
            // }
        }

        bool UpdateLubanDataInternal()
        {
            var assetPath = "Assets/_GameClient/EditorRes/Luban/LubanBinary.asset";
            var lubanExporter =
                AssetDatabase.LoadAssetAtPath<Luban.Editor.LubanExportConfig>(assetPath);
            if (lubanExporter == null)
            {
                Debug.LogError($"没找到luban导出配置 {assetPath}");
                return false;
            }

            lubanExporter.Gen();
            AssetDatabase.Refresh();
            return true;
        }

        [BoxGroup("更 新 配 置")]
        [Button("一 键 更 新(包括以上全部)", ButtonSizes.Large), GUIColor(0.0f, 1.0f, 0.0f)]
        void UpdateByOneClick()
        {
            //
            //SpriteAtlasHelper.CreateAtlasData();
            SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
            //
            //SpriteManagerToolHelper.GenerateSpriteDataConfig();
            //
            TextCollector.Collect();
            //
            UpdateLubanDataInternal();

            //AddressableImporter.FolderImporter.ReimportFolders(new[] { "Assets" }, false);
            EditorUtility.DisplayDialog("提示", "配置生成完成", "OK");
        }

        [BoxGroup("资 源 打 包", order: 3)]
        [Button("编 译 热 更 新 代 码", ButtonSizes.Large), GUIColor(0.0f, 1.0f, 0.0f)]
        void UpdateCodes()
        {
            //CompileDllCommand.CompileDllActiveBuildTarget();
            //HybridCLRTool.CopyDll();

            //HybridCLRTool.BuildHotfixAssembly();
            //HybridCLRTool.CopyDll();

            EditorUtility.DisplayDialog("提示", "热更代码编译完成", "OK");
        }

        [BoxGroup("资 源 打 包")]
        [Button("编 译 Addressable", ButtonSizes.Large), GUIColor(0.0f, 1.0f, 0.0f)]
        void BuildAddressable()
        {
            // AddressableImporter.FolderImporter.ReimportFolders(new[] { "Assets" }, false);
            // var success = AddressableHelper.BuildAddressable();
            // if (!success)
            // {
            //     EditorUtility.DisplayDialog("提示", "资源打包失败!!!", "OK");
            //     return;
            // }
            //
            // EditorUtility.DisplayDialog("提示", "资源打包成功~~~", "OK");
        }

        // [BoxGroup("资源打包")]
        // [Button("编译Addressable+处理为首包资源", ButtonSizes.Large), GUIColor(0.0f, 1.0f, 0.0f)]
        // void BuildAddressableAndInPack()
        // {
        //     UpdateAddressable();
        //     var success = false;
        //     success = AddressableHelper.BuildAddressable();
        //     if (!success)
        //     {
        //         EditorUtility.DisplayDialog("提示", "编译失败!!!", "OK");
        //         return;
        //     }
        //     success =  AddressableHelper.ProcessForInPack();
        //     if (!success)
        //     {
        //         EditorUtility.DisplayDialog("提示", "处理失败!!!", "OK");
        //         return;
        //         
        //     }
        //     EditorUtility.DisplayDialog("提示", "跟包资源编译成功~~~", "OK");
        // }

        [BoxGroup("资源同步", order: 4)] [LabelText("同步目录")]
        public List<string> syncPathList = new List<string>();

        [BoxGroup("资源同步")]
        [Button("同 步 资 源 文 件", ButtonSizes.Large), GUIColor(0.0f, 1.0f, 0.0f)]
        void SyncAssets()
        {
            var srcPath =
                $"{Application.dataPath}/../Library/com.unity.addressables/aa/{EditorUserBuildSettings.activeBuildTarget}";
            foreach (var dstPath in syncPathList)
            {
                if (Directory.Exists(dstPath))
                    Directory.Delete(dstPath, true);
                FileUtil.CopyFileOrDirectory(srcPath, dstPath);
            }

            EditorUtility.DisplayDialog("提示", "同步完成~~~", "OK");
        }


        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}