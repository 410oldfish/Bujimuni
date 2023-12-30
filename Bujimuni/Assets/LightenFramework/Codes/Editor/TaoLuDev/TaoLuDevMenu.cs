using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighten.Editor
{
    public static class TaoLuDevMenu
    {
        [MenuItem(LightenEditorConst.MENU_ROOT + "/开发面板", priority = 0)]
        public static void OpenDevWindow()
        {
            var window = EditorWindow.GetWindow<TaoLuDevWindow>("开发面板");
            window.Show();
        }
        
        [MenuItem(LightenEditorConst.MENU_ROOT + "/导出LightenFramework", priority = 9999)]
        public static void Export()
        {
            var outputDir = $"{Application.dataPath}/../ExportPackages";
            DirectoryExtension.GenerateDirectory(outputDir);
            Export(outputDir);
        }

        //这里是定义需要导出的资源路径
        private static string[] exportAssetPaths = new[]
        {
            "Assets/_GameClient",
            "Assets/EditorResources",
            "Assets/LightenFramework",
        };

        private static void Export(string outputDir)
        {
            var outputPath = $"{outputDir}/LightenFramework-{DateTime.Now.ToString("yyyy-MM-dd")}.unitypackage";
            try
            {
                DirectoryExtension.GenerateDirectory(outputPath);
                AssetDatabase.ExportPackage(exportAssetPaths, outputPath, ExportPackageOptions.Interactive | ExportPackageOptions.Recurse);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return;
            }

            Debug.Log($"Export package complete: {outputPath}");
            EditorUtility.DisplayDialog("导出成功", $"导出成功，路径为：{outputPath}", "确定");
        }
        
        // [MenuItem("Assets/GameObject快捷选项/生成GameObject代码", priority = -90)]
        // [MenuItem("GameObject/GameObject快捷选项/生成GameObject代码", priority = -90)]
        // static void GeneratePrefabViewCode()
        // {
        //     var selectedGameObject = Selection.activeGameObject;
        //     if (selectedGameObject == null)
        //         return;
        //     var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(selectedGameObject);
        //     GoScriptGenerator.Instance.AddControllerToGameObject(sourceGameObject);
        //     AssetDatabase.Refresh();
        // }
        //
        // [MenuItem("Assets/GameObject快捷选项/编辑GameObject代码", priority = -90)]
        // [MenuItem("GameObject/GameObject快捷选项/编辑GameObject代码", priority = -90)]
        // static void EditPrefabViewCode()
        // {
        //     var selectedGameObject = Selection.activeGameObject;
        //     if (selectedGameObject == null)
        //         return;
        //     var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(selectedGameObject);
        //     var scriptDir = GoScriptGenerator.Instance.GetScriptDir(sourceGameObject, GoScriptGenerator.OUTPUT_FOLDER_NAME);
        //     var filePath = $"{scriptDir}/{sourceGameObject.name}.Controller.cs";
        //     if (!File.Exists(filePath))
        //     {
        //         EditorUtility.DisplayDialog("提示", $"{filePath}不存在", "OK");
        //         return;
        //     }
        //     InternalEditorUtility.OpenFileAtLineExternal(filePath, 0);
        // }
        
    }
}