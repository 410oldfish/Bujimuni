using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Lighten;

namespace Lighten.Editor
{
    public static class TUIEditorHelper
    {
        //获取TUIEditorSetting
        public static TUIEditorSetting GetTUIEditorSetting()
        {
            return AssetDatabaseExtension.GetOrCreateScriptableObject<TUIEditorSetting>(TUIEditorConst.UI_SETTING_PATH);
        }

        //是否为有效UI名称
        public static bool IsValidUIName(GameObject gameObject)
        {
            var prefabName = gameObject.name;
            var type = UIHelper.GetEntityType(prefabName);
            if (type == EUIEntityType.None)
            {
                return false;
            }
            return true;
        }

        //是否需要重新生成列表
        public static bool IsNeedToUpdateUIConfig(TUIEditorSetting editorSetting, GameObject gameObject)
        {
            var prefabName = gameObject.name;
            var type = UIHelper.GetEntityType(prefabName);
            if (type == EUIEntityType.None)
                return false;
            var uiConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
            switch (type)
            {
                case EUIEntityType.Window:
                {
                    var data = uiConfig.WindowInfos.Find(d => d.PrefabName == prefabName);
                    return data == null;
                }
                case EUIEntityType.Widget:
                {
                    var data = uiConfig.WidgetInfos.Find(d => d.PrefabName == prefabName);
                    return data == null;
                }
            }

            return false;
        }

        //绑定界面
        public static bool BindUIVariables(TUIEditorSetting editorSetting, GameObject gameObject)
        {
            var scriptDir = UIScriptGenerator.Instance.GetScriptDir(gameObject, editorSetting.uiControllerFolder);
            if (string.IsNullOrEmpty(scriptDir))
                return false;
            var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(gameObject);
            var prefixName = UIHelper.GetPrefixName(sourceGameObject.name);
            switch (prefixName)
            {
                case UIPrefix.Window:
                    UIScriptGenerator.Instance.Generate(sourceGameObject, "UIWindowTemplate", editorSetting.uiControllerFolder);
                    break;
                case UIPrefix.Widget:
                {
                    UIScriptGenerator.Instance.Generate(sourceGameObject, "UIWidgetTemplate", editorSetting.uiControllerFolder);
                    var widgetMono = sourceGameObject.GetComponent<UIWidgetMono>();
                    if (widgetMono == null)
                    {
                        widgetMono = sourceGameObject.AddComponent<UIWidgetMono>();
                    }
                    widgetMono.WidgetName = sourceGameObject.name;
                    EditorUtility.SetDirty(sourceGameObject);
                    AssetDatabase.SaveAssetIfDirty(sourceGameObject);
                }
                    break;
                default:
                    throw new Exception($"未知的UI类型:{prefixName}");
            }
            
            return true;
        }

        //获取界面脚本
        public static string GetUIScriptPath(TUIEditorSetting editorSetting, GameObject gameObject)
        {
            var scriptDir = UIScriptGenerator.Instance.GetScriptDir(gameObject, editorSetting.uiControllerFolder);
            if (string.IsNullOrEmpty(scriptDir))
                return string.Empty;
            var filePath = $"{scriptDir}/{gameObject.name}.cs";
            if (!File.Exists(filePath))
            {
                EditorUtility.DisplayDialog("提示", $"脚本不存在{filePath}", "OK");
                return string.Empty;
            }

            return filePath;
        }

        #region 生成UIConfig

        /*
         * 1.找到所有Dlg开头的prefab
         * 2.根据字串排序,生成WindowId
         * 3.访问已经存在的UIConfig,保留除了WindowId之外的数据
         * 4.生成新的UIConfig
         */
        public static void GenerateUIConfig()
        {
            var windowPrefabs = FindWindowPrefabs(TUIEditorConst.UI_PREFAB_PATH, UIPrefix.Window);
            windowPrefabs.Sort((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));

            var widgetPrefabs = FindWidgetPrefabs(TUIEditorConst.UI_PREFAB_PATH, UIPrefix.Widget);
            widgetPrefabs.Sort((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
            
            var uiConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
            var lastDatas = new Dictionary<string, UIConfig.WindowInfo>();
            foreach (var data in uiConfig.WindowInfos)
            {
                if (lastDatas.ContainsKey(data.PrefabName))
                {
                    Debug.LogError($"名字重复{data.PrefabName}");
                    continue;
                }
                lastDatas.Add(data.PrefabName, data);
            }

            int windowNum = 1;
            //
            uiConfig.WindowInfos.Clear();
            foreach (var windowPrefab in windowPrefabs)
            {
                var data = new UIConfig.WindowInfo();
                data.WindowName = windowPrefab.name;
                data.PrefabName = windowPrefab.name;
                if (lastDatas.ContainsKey(data.PrefabName))
                {
                    var lastData = lastDatas[data.PrefabName];
                    data.Clone(lastData);
                }
                else
                {
                    data.SortLayer = EUISortLayer.Normal;
                    data.Depth = 0;
                    data.MaskName = string.Empty;
                    data.IsCloseByClickBlank = false;
                }

                uiConfig.WindowInfos.Add(data);
            }

            //
            uiConfig.WidgetInfos.Clear();
            foreach (var widgetPrefab in widgetPrefabs)
            {
                var data = new UIConfig.WidgetInfo();
                data.WidgetName = widgetPrefab.name;
                data.PrefabName = widgetPrefab.name;
                uiConfig.WidgetInfos.Add(data);
            }
            
            //生成ID段
            //GenerateWindowIdDefine(editorSetting, uiConfig);

            // for (int i = 0; i < uiConfig.WindowInfos.Count; ++i)
            // {
            //     var data = uiConfig.WindowInfos[i];
            //     GenerateUIScriptOfWindow(editorSetting, data.PrefabName, windowPrefabs[i]);
            // }
            //
            // for (int i = 0; i < uiConfig.WidgetInfos.Count; ++i)
            // {
            //     var data = uiConfig.WidgetInfos[i];
            //     GenerateUIScriptOfWidget(editorSetting, data.PrefabName, widgetPrefabs[i]);
            // }
            
            EditorUtility.SetDirty(uiConfig);
            AssetDatabase.SaveAssetIfDirty(uiConfig);
        }

        //搜索所有的window prefab
        private static List<GameObject> FindWindowPrefabs(string folderPath, string prefixName)
        {
            var result = new List<GameObject>();
            var assetPaths = AssetDatabaseExtension.SearchAssetPaths(new[] { folderPath }, "*.prefab",
                assetPath =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(assetPath);
                    return fileName.StartsWith(prefixName);
                });
            foreach (var assetPath in assetPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab == null)
                    continue;
                if (prefab.GetComponent<Canvas>() == null)
                {
                    Debug.LogError($"Window必须有Canvas组件,请检查:{assetPath}");
                    continue;
                }

                result.Add(prefab);
            }

            return result;
        }

        //搜索所有的widget prefab
        private static List<GameObject> FindWidgetPrefabs(string folderPath, string prefixName)
        {
            var result = new List<GameObject>();
            var assetPaths = AssetDatabaseExtension.SearchAssetPaths(new[] { folderPath }, "*.prefab",
                assetPath =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(assetPath);
                    return fileName.StartsWith(prefixName);
                });
            foreach (var assetPath in assetPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab == null)
                    continue;
                if (prefab.GetComponent<Canvas>() != null)
                {
                    Debug.LogError($"Widget不需要Canvas组件,请检查:{assetPath}");
                    continue;
                }

                result.Add(prefab);
            }

            return result;
        }
        
        #endregion
    }
}