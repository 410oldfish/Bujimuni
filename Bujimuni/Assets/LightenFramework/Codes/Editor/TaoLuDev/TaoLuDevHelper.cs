using System.Collections.Generic;
using System.IO;
using System.Linq;
using Luban.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lighten.Editor
{
    public static class TaoLuDevHelper
    {
        //生成luban
        private static string[] m_lubanExportNames = new string[] { "LubanCBytes", "LubanCJson", "LubanSBytes" };

        public static string GenerateLuban()
        {
            var rootPath = "Assets/LightenFramework/Codes/Editor/LubanUnityGUI";
            foreach (var lubanExportName in m_lubanExportNames)
            {
                var assetPath = $"{rootPath}/{lubanExportName}.asset";
                var lubanExportConfig = AssetDatabase.LoadAssetAtPath<LubanExportConfig>(assetPath);
                if (lubanExportConfig == null)
                {
                    Debug.LogError($"没有找到 {assetPath}");
                    continue;
                }

                var errorMsg = lubanExportConfig.GenReturnErrorMsg();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    Debug.LogError(errorMsg);
                    return errorMsg;
                }
            }

            return string.Empty;
        }

        //生成AddressableGroups
        public static void GenerateAddressableGroups()
        {
            // var importSettingsList = AddressableImportSettingsList.Instance;
            // if (importSettingsList == null)
            //     return;
            // var hasRuleSettingsList = importSettingsList.EnabledSettingsList.Where(s => s.rules.Count > 0).ToList();
            // if (hasRuleSettingsList.Count <= 0)
            //     return;
            // var ruleFolders = new List<string>();
            // foreach (var hasRuleSetting in hasRuleSettingsList)
            // {
            //     foreach (var rule in hasRuleSetting.rules)
            //     {
            //         var folderPath = string.Empty;
            //         switch (rule.matchType)
            //         {
            //             case AddressableImportRuleMatchType.Wildcard:
            //                 folderPath = Path.GetDirectoryName(rule.path);
            //                 folderPath = folderPath.Replace("\\", "/");
            //                 break;
            //             case AddressableImportRuleMatchType.Regex:
            //                 folderPath = GetFolderPathByRegexStr(rule.path);
            //                 break;
            //         }
            //
            //         Debug.Log($"rule.path = {rule.path} folderPath = {folderPath}");
            //         if (string.IsNullOrEmpty(folderPath))
            //             continue;
            //         ruleFolders.Add(folderPath);
            //     }
            // }
            //
            // AddressableImporter.FolderImporter.ReimportFolders(ruleFolders, false);
        }

        //将一个正则表达式转成目录
        private static List<char> m_regexChars = new List<char>
        {
            '*',
            '?',
            '^',
            '[',
            ']',
            '<',
            '>',
            '$'
        };

        private static List<string> m_tempStrs = new List<string>();

        private static string GetFolderPathByRegexStr(string str)
        {
            str = str.Replace("\\", "/");
            if (str.StartsWith("^"))
            {
                str = str.Substring(1);
            }

            var array = str.Split('/');
            m_tempStrs.Clear();
            for (int i = 0; i < array.Length; ++i)
            {
                var splitStr = array[i];
                if (m_regexChars.Any(c => splitStr.Contains(c)))
                    break;
                if (m_tempStrs.Count > 0)
                {
                    m_tempStrs.Add("/");
                }

                m_tempStrs.Add(splitStr);
            }

            if (m_tempStrs.Count <= 0)
                return string.Empty;
            return string.Concat(m_tempStrs);
        }

        //生成精灵
        public static bool GenerateSpriteConfig()
        {
            return SpriteMgrEditorHelper.GenerateSpriteDataConfig();
        }

        //生成图集(UI自动图集)
        public static void GenerateUIAtlas()
        {
            TUIAtlasMakder.GenerateSpriteAtlasOfUI();
        }

        // add macros
        public static void AddMacros(params string[] defineNames)
        {
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            var curDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var listOfDefines = curDefines.Split(';').ToList();
            foreach (var defineName in defineNames)
            {
                if (!listOfDefines.Contains(defineName))
                {
                    listOfDefines.Add(defineName);
                }
            }

            var defines = string.Join(";", listOfDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        //remove macros
        public static void RemoveMacros(params string[] defineNames)
        {
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            var curDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            var listOfDefines = curDefines.Split(';').ToList();
            foreach (var defineName in defineNames)
            {
                if (listOfDefines.Contains(defineName))
                {
                    listOfDefines.Remove(defineName);
                }
            }

            var defines = string.Join(";", listOfDefines);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defines);
        }

        #region 帮助文档

        private static Dictionary<string, EditorWindow> m_openedDocumentWindows = new Dictionary<string, EditorWindow>();

        private static EditorWindow OpenDocument(string fileName)
        {
            if (m_openedDocumentWindows.ContainsKey(fileName))
            {
                return null;
            }

            var assetPath = $"{LightenEditorConst.LIGHTEN_EDITOR_DOCUMENT_DIR}/{fileName}.md";
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            if (textAsset == null)
            {
                EditorUtility.DisplayDialog("提示", $"没有找到帮助文档 {assetPath}", "确定");
                return null;
            }

            var window = OdinEditorWindow.InspectObject(textAsset);
            window.minSize = new Vector2(700, 600);
            return window;
        }

        private static void HideDocument(string fileName)
        {
            if (m_openedDocumentWindows.ContainsKey(fileName))
            {
                m_openedDocumentWindows[fileName].Close();
                m_openedDocumentWindows.Remove(fileName);
            }
        }

        public static EditorWindow DrawHelpWindow(Rect position, string helpDocumentName)
        {
            var width = 50;
            position.x -= width;
            position.width = width;
            var color = GUI.color;
            GUI.color = Color.green;
            EditorWindow window = null;
            if (GUI.Button(position, "帮助"))
            {
                if (m_openedDocumentWindows.ContainsKey(helpDocumentName))
                {
                    HideDocument(helpDocumentName);
                }
                else
                {
                    window = OpenDocument(helpDocumentName);
                }
            }

            GUI.color = color;
            return window;
        }

        #endregion
        
        #region 首次运行检查
        
        //
        static int GetOrCreateSortingLayerByName(string layerName)
        {
            foreach (var layer in SortingLayer.layers)
            {
                if (layer.name == layerName)
                {
                    return layer.id;
                }
            }
            //
            var asset = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");
            var tagManager = new SerializedObject(asset); 
            var property = tagManager.FindProperty("m_SortingLayers");
            if (property!= null)
            {
                property.InsertArrayElementAtIndex(property.arraySize);
                var element = property.GetArrayElementAtIndex(property.arraySize - 1);
                element.FindPropertyRelative("name").stringValue = layerName;
                element.FindPropertyRelative("uniqueID").intValue = property.arraySize - 1;
                tagManager.ApplyModifiedProperties();
                
                AssetDatabase.SaveAssetIfDirty(asset);
            }
            //
            foreach (var layer in SortingLayer.layers)
            {
                if (layer.name == layerName)
                {
                    return layer.id;
                }
            }
            return 0;
        }

        [InitializeOnLoadMethod]
        [MenuItem("Assets/CheckOnLoad")]
        static void CheckOnLoad()
        {
            Debug.Log("CheckOnLoad");
            int sortingLayerID = GetOrCreateSortingLayerByName("UI");
            if (sortingLayerID == 0)
                return;
            var assetPath = "Assets/LightenFramework/Prefabs/UIRoot.prefab";
            var uiRoot = PrefabUtility.LoadPrefabContents(assetPath);
            if (uiRoot != null)
            {
                var canvasList = uiRoot.GetComponentsInChildren<Canvas>(true);
                foreach (var canvas in canvasList)
                {
                    //Debug.Log($"{canvas.sortingLayerID} => {sortingLayerID}");
                    canvas.sortingLayerID = sortingLayerID;
                }
                PrefabUtility.SaveAsPrefabAsset(uiRoot, assetPath);
                PrefabUtility.UnloadPrefabContents(uiRoot);
            }
            
        }
        
        #endregion
        
        #region 编辑器运行游戏

        [InitializeOnLoadMethod]
        static void RunSceneOnLoadMethod()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                //Debug.Log(state);
                if (state == PlayModeStateChange.ExitingEditMode)
                {
                    var scene = SceneManager.GetActiveScene();
                    Debug.Log($"{scene.path}");
                    if (scene.path.Contains("Assets/_GameClient/Bundles/Scenes"))
                    {
                        EditorPrefs.SetString("LastScenePath", scene.path);
                        EditorSceneManager.OpenScene("Assets/_GameClient/LaunchScene.unity");
                    }
                    
                }
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    var lastScenePath = EditorPrefs.GetString("LastScenePath", string.Empty);
                    Debug.Log(lastScenePath);
                    if (!string.IsNullOrEmpty(lastScenePath))
                    {
                        EditorSceneManager.OpenScene(lastScenePath);
                        EditorPrefs.DeleteKey("LastScenePath");
                    }
                }
            };
        }
        #endregion
    }
}