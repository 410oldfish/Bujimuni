using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
//using HybridCLR.Editor;//TODO:暂时不用HybridCLR
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;

namespace Lighten.Editor
{
    public static class HybridCLRTool
    {
        public static string CONFIG_PATH =
            $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/ScriptableObjects/Runtime/HybridCLRAssemblyConfig.asset";

        public static string PROJECT_CODE_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/Raw/Codes";
        public static string PROJECT_CODEAOT_PATH = $"{LightenEditorConst.LIGHTEN_PROJECT_PATH}/Raw/CodesAOT";
        
        public static string HOTFIX_ASSEMBLY_BUILD_PATH = $"{Application.dataPath}/../HybridCLRData/HotfixAssembly";

        //[MenuItem("HybridCLR/复制生成的dll到项目工程", priority = 999)]
        public static void CopyDll()
        {
            var config = AssetDatabaseExtension.GetOrCreateScriptableObject<HybridCLRAssemblyConfig>(CONFIG_PATH);
            //清空文件夹
            Directory.Delete(PROJECT_CODE_PATH, true);
            Directory.CreateDirectory(PROJECT_CODE_PATH);
            Directory.Delete(PROJECT_CODEAOT_PATH, true);
            Directory.CreateDirectory(PROJECT_CODEAOT_PATH);
            //复制补元dll
            CopyAOTAssembliesToProject(config);
            //复制热更dll
            //CopyHotUpdateAssembliesToProject(config);
            CopyHotfixAssemblyToProject();
            
            AssetDatabase.Refresh();
        }

        //TODO:暂时不用HybridCLR
        //[MenuItem("HybridCLR/配置程序集", priority = 1000)]
        public static void EditHybridCLRAssemblyNames()
        {
            var config = AssetDatabaseExtension.GetOrCreateScriptableObject<HybridCLRAssemblyConfig>(CONFIG_PATH);
            Selection.activeObject = config;
        }

        public static void CopyAOTAssembliesToProject(HybridCLRAssemblyConfig config)
        {
            //TODO:暂时不用HybridCLR
            // var target = EditorUserBuildSettings.activeBuildTarget;
            // string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            // string aotAssembliesDstDir = PROJECT_CODEAOT_PATH;
            //
            // foreach (var dll in config.AOTMetaAssemblyNames)
            // {
            //     string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
            //     if (!File.Exists(srcDllPath))
            //     {
            //         Debug.LogError(
            //             $"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
            //         continue;
            //     }
            //     string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
            //     File.Copy(srcDllPath, dllBytesPath, true);
            //     Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            // }
        }

        public static void CopyHotUpdateAssembliesToProject(HybridCLRAssemblyConfig config)
        {
            //TODO:暂时不用HybridCLR
            // var target = EditorUserBuildSettings.activeBuildTarget;
            //
            // string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            // string hotfixAssembliesDstDir = PROJECT_CODE_PATH;
            // foreach (var dll in config.HotfixAssemblyNames)
            // {
            //     string dllPath = $"{hotfixDllSrcDir}/{dll}.dll";
            //     string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.dll.bytes";
            //     File.Copy(dllPath, dllBytesPath, true);
            //
            //     Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
            //
            //     string pdbPath = $"{hotfixDllSrcDir}/{dll}.pdb";
            //     string pdbBytesPath = $"{hotfixAssembliesDstDir}/{dll}.pdb.bytes";
            //     File.Copy(pdbPath, pdbBytesPath, true);
            //
            //     Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix pdb {pdbPath} -> {pdbBytesPath}");
            // }
        }

        //编译程序集
        public static void BuildHotfixAssembly()
        {
            //TODO:暂时不用HybridCLR
            // //TODO:这里可以根据选择debug和release模式
            // var codeOptimization = CodeOptimization.Release;
            // var flags = AssemblyBuilderFlags.None;
            // var additionalReferences = new List<string>();
            // //这里要把热更新程序集的引用排除掉
            // var excludeReferences = new List<string>();
            // foreach (var hotUpdateAssemblyDefinition in HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions)
            // {
            //     excludeReferences.Add($"Library/ScriptAssemblies/{hotUpdateAssemblyDefinition.name}.dll");
            // }
            // foreach (var hotUpdateAssemblyDefinition in HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions)
            // {
            //     var assemblyName = hotUpdateAssemblyDefinition.name;
            //     var assemblyPath = AssetDatabase.GetAssetPath(hotUpdateAssemblyDefinition);
            //     var assemblyCode = Path.GetDirectoryName(assemblyPath);
            //     Debug.Log($"{assemblyName} {assemblyCode}");
            //     LightenEditor.AssemblyBuildHelper.BuildAssembly(HOTFIX_ASSEMBLY_BUILD_PATH, assemblyName,
            //         new List<string>() { assemblyCode },
            //         additionalReferences, codeOptimization, flags, excludeReferences);
            //     additionalReferences.Add(Path.Combine(HOTFIX_ASSEMBLY_BUILD_PATH, $"{assemblyName}.dll"));
            // }
        }
        public static void CopyHotfixAssemblyToProject()
        {
            //TODO:暂时不用HybridCLR
            // foreach (var hotUpdateAssemblyDefinition in HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions)
            // {
            //     var assemblyName = hotUpdateAssemblyDefinition.name;
            //     var assemblyPath = Path.Combine(HOTFIX_ASSEMBLY_BUILD_PATH, $"{assemblyName}.dll");
            //     var dllBytesPath = $"{PROJECT_CODE_PATH}/{assemblyName}.dll.bytes";
            //     File.Copy(assemblyPath, dllBytesPath, true);
            //
            //     Debug.Log($"[CopyHotfixAssemblyToProject] copy hotfix dll {assemblyPath} -> {dllBytesPath}");
            // }
        }
        
#if ENABLE_CODES
        //[MenuItem("LightenFramework/代 码 模 式/关闭本地代码模式", priority = 0)]
        public static void RemoveEnableCodes()
        {
            EnableCodes(false);
        }
#else
        //[MenuItem("LightenFramework/代 码 模 式/开启本地代码模式", priority = 0)]
        public static void AddEnableCodes()
        {
            EnableCodes(true);
        }
        
        //[MenuItem("LightenFramework/代 码 模 式/编译代码 _F5", priority = 0)]
        public static void BuildHotfixCode()
        {
            BuildHotfixAssembly();
            CopyHotfixAssemblyToProject();
            
            EditorUtility.DisplayDialog("提示", "编译完成", "OK");
        }
#endif

        public static void EnableCodes(bool enable)
        {
            string defines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var ss = defines.Split(';').ToList();
            if (enable)
            {
                if (ss.Contains("ENABLE_CODES"))
                {
                    return;
                }
                ss.Add("ENABLE_CODES");
            }
            else
            {
                if (!ss.Contains("ENABLE_CODES"))
                {
                    return;
                }
                ss.Remove("ENABLE_CODES");
            }
            defines = string.Join(";", ss);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, defines);
            AssetDatabase.SaveAssets();
        }
    }
}