using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Lighten.Editor
{
    public class TUIEditorWindow: OdinMenuEditorWindow
    {
        #region GeneralSetting

        public class GeneralSetting
        {
            public void Init()
            {
            }

            [HorizontalGroup()]
            [Button("UIEditorSetting 配 置", ButtonSizes.Large), GUIColor(0, 1, 0)]
            public void SelectEditorTUISetting()
            {
                Selection.activeObject = TUIEditorHelper.GetTUIEditorSetting();
            }

            [Button("生 成 界 面", ButtonSizes.Large), GUIColor(0, 1, 0)]
            public void GenerateUIConfig()
            {
                //var editorSetting = TUIEditorHelper.GetTUIEditorSetting(TUIEditorPrefs.GetEditorSettingPath());
                TUIEditorHelper.GenerateUIConfig();
                EditorUtility.DisplayDialog("提示", "生成界面配置成功", "确定");
            }
        }

        #endregion

        #region WindowsLayerSetting

        public class WindowsLayerSetting
        {
            [LabelText("窗 口 层 级 列 表")]
            [CustomValueDrawer("WindowLayerInfoSettingDrawer")]
            public List<UIConfig.WindowLayerInfo> WindowLayerInfoList = new List<UIConfig.WindowLayerInfo>();

            [Button("保 存 数 据", ButtonSizes.Large), GUIColor(0, 1, 0)]
            void SaveData()
            {
                var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
                UIConfig.WindowLayerList.Clear();
                UIConfig.WindowLayerList.AddRange(this.WindowLayerInfoList);
                EditorUtility.SetDirty(UIConfig);
                AssetDatabase.SaveAssetIfDirty(UIConfig);
            }

            public void Init()
            {
                var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
                this.WindowLayerInfoList.Clear();
                this.WindowLayerInfoList.AddRange(UIConfig.WindowLayerList);
            }

            private UIConfig.WindowLayerInfo WindowLayerInfoSettingDrawer(UIConfig.WindowLayerInfo value)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("层级", GUILayout.Width(30));
                value.Layer = (EUISortLayer)EditorGUILayout.EnumPopup(value.Layer);

                EditorGUILayout.LabelField("剔除模式", GUILayout.Width(50));
                value.CullMode = (EUICullMode)EditorGUILayout.EnumPopup(value.CullMode);

                EditorGUILayout.EndHorizontal();
                return value;
            }
        }

        #endregion

        #region WindowsSetting

        public class WindowsSetting
        {
            [LabelText("界 面 搜 索")]
            [OnValueChanged("SearchWindow")]
            public string FilterWindowName;

            [LabelText("界 面 列 表")]
            [CustomValueDrawer("WindowInfoDrawer")]
            public List<UIConfig.WindowInfo> WindowInfos = new List<UIConfig.WindowInfo>();

            [Button("刷 新 列 表", ButtonSizes.Large), GUIColor(0, 1, 0)]
            void RefreshList()
            {
                this.WindowInfos.Sort(SortMethod);
            }

            [Button("保 存 数 据", ButtonSizes.Large), GUIColor(0, 1, 0)]
            void SaveData()
            {
                var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
                for (int i = 0; i < UIConfig.WindowInfos.Count; ++i)
                {
                    var windowInfo = this.WindowInfos.Find(item => item.WindowName == UIConfig.WindowInfos[i].WindowName);
                    if (windowInfo != null)
                    {
                        UIConfig.WindowInfos[i] = windowInfo;
                    }
                }

                EditorUtility.SetDirty(UIConfig);
                AssetDatabase.SaveAssetIfDirty(UIConfig);
            }

            void SearchWindow()
            {
                LoadWindowInfos(this.FilterWindowName);
            }

            public void Init()
            {
                LoadWindowInfos(this.FilterWindowName);
            }

            private void LoadWindowInfos(string filterName = null)
            {
                var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
                this.WindowInfos.Clear();
                if (string.IsNullOrEmpty(filterName))
                {
                    this.WindowInfos.AddRange(UIConfig.WindowInfos);
                }
                else
                {
                    filterName = filterName.ToLower();
                    foreach (var windowInfo in UIConfig.WindowInfos)
                    {
                        if (windowInfo.PrefabName.ToLower().Contains(filterName))
                        {
                            this.WindowInfos.Add(windowInfo);
                        }
                    }
                }

                //根据深度排序
                this.WindowInfos.Sort(SortMethod);
            }

            private int SortMethod(UIConfig.WindowInfo a, UIConfig.WindowInfo b)
            {
                if (a.SortLayer != b.SortLayer)
                {
                    return a.SortLayer.CompareTo(b.SortLayer);
                }

                if (a.Depth != b.Depth)
                {
                    return a.Depth.CompareTo(b.Depth);
                }

                return String.CompareOrdinal(a.PrefabName, b.PrefabName);
            }

            private UIConfig.WindowInfo WindowInfoDrawer(UIConfig.WindowInfo value)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(value.PrefabName, GUILayout.MinWidth(60));
                value.SortLayer = (EUISortLayer)EditorGUILayout.EnumPopup(value.SortLayer, GUILayout.Width(80));
                EditorGUILayout.LabelField("深度", GUILayout.Width(30));
                value.Depth = EditorGUILayout.IntField(value.Depth, GUILayout.Width(40));
                EditorGUILayout.LabelField("遮罩", GUILayout.Width(30));
                value.IsShowMask = EditorGUILayout.Toggle(value.IsShowMask, GUILayout.Width(12));
                if (value.IsShowMask)
                {
                    value.MaskName = EditorGUILayout.TextField(value.MaskName, GUILayout.Width(60));
                }
                else
                {
                    EditorGUILayout.LabelField(value.MaskName, GUILayout.Width(60));
                }

                EditorGUILayout.LabelField("|点空关", GUILayout.Width(45));
                value.IsCloseByClickBlank = EditorGUILayout.Toggle(value.IsCloseByClickBlank, GUILayout.Width(15));

                #region 操作按钮

                if (EditorGUILayout.LinkButton("绑定"))
                {
                    Debug.Log($"{value.PrefabName} 绑定");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var success = TUIEditorHelper.BindUIVariables(editorSetting, prefab);
                    if (success)
                    {
                        AssetDatabase.Refresh();
                    }
                }

                if (EditorGUILayout.LinkButton("代码"))
                {
                    Debug.Log($"{value.PrefabName} 代码");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var scriptPath = TUIEditorHelper.GetUIScriptPath(editorSetting, prefab);
                    if (!string.IsNullOrEmpty(scriptPath))
                    {
                        InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 0);
                    }
                }

                if (EditorGUILayout.LinkButton("prefab"))
                {
                    Debug.Log($"{value.PrefabName} 预制体");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);

                    m_waitToOpenPrefab = prefab;
                }

                #endregion

                EditorGUILayout.EndHorizontal();

                return value;
            }
        }

        #endregion

        #region WidgetsSetting

        public class WidgetsSetting
        {
            [LabelText("组 件 搜 索")]
            [OnValueChanged("SearchWindow")]
            public string FilterWindowName;

            [LabelText("组 件 列 表")]
            [CustomValueDrawer("WidgetInfoDrawer")]
            public List<UIConfig.WidgetInfo> WidgetInfos = new List<UIConfig.WidgetInfo>();

            [Button("刷 新 列 表", ButtonSizes.Large), GUIColor(0, 1, 0)]
            void RefreshList()
            {
                this.WidgetInfos.Sort(SortMethod);
            }

            void SearchWindow()
            {
                this.LoadWidgetInfos();
            }

            public void Init()
            {
                this.LoadWidgetInfos();
            }

            private void LoadWidgetInfos(string filterName = null)
            {
                var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(TUIEditorConst.UI_CONFIG_PATH);
                this.WidgetInfos.Clear();
                if (string.IsNullOrEmpty(filterName))
                {
                    this.WidgetInfos.AddRange(UIConfig.WidgetInfos);
                }
                else
                {
                    filterName = filterName.ToLower();
                    foreach (var widgetInfo in UIConfig.WidgetInfos)
                    {
                        if (widgetInfo.PrefabName.ToLower().Contains(filterName))
                        {
                            this.WidgetInfos.Add(widgetInfo);
                        }
                    }
                }

                //根据深度排序
                this.WidgetInfos.Sort(SortMethod);
            }

            private int SortMethod(UIConfig.WidgetInfo a, UIConfig.WidgetInfo b)
            {
                return String.CompareOrdinal(a.PrefabName, b.PrefabName);
            }

            private UIConfig.WidgetInfo WidgetInfoDrawer(UIConfig.WidgetInfo value)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(value.PrefabName);

                #region 操作按钮

                if (EditorGUILayout.LinkButton("绑定"))
                {
                    Debug.Log($"{value.PrefabName} 绑定");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var success = TUIEditorHelper.BindUIVariables(editorSetting, prefab);
                    if (success)
                    {
                        AssetDatabase.Refresh();
                    }
                }

                if (EditorGUILayout.LinkButton("编辑"))
                {
                    Debug.Log($"{value.PrefabName} 编辑");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var scriptPath = TUIEditorHelper.GetUIScriptPath(editorSetting, prefab);
                    if (!string.IsNullOrEmpty(scriptPath))
                    {
                        InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 0);
                    }
                }

                if (EditorGUILayout.LinkButton("选中"))
                {
                    Debug.Log($"{value.PrefabName} 选中");
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value.PrefabName, TUIEditorConst.UI_PREFAB_PATH);
                    Selection.activeObject = prefab;
                }

                #endregion

                EditorGUILayout.EndHorizontal();
                return value;
            }
        }

        #endregion

        #region CommonsSetting

        public class CommonsSetting
        {
            [LabelText("组 件 搜 索")]
            [OnValueChanged("SearchWindow")]
            public string FilterCommonName;

            [LabelText("组 件 列 表")]
            [CustomValueDrawer("WidgetInfoDrawer")]
            public List<string> CommonInfos = new List<string>();

            [Button("刷 新 列 表", ButtonSizes.Large), GUIColor(0, 1, 0)]
            void RefreshList()
            {
                this.CommonInfos.Sort(SortMethod);
            }

            // void SearchWindow()
            // {
            //     this.LoadCommonInfos(this.FilterCommonName);
            // }
            //
            // public void Init()
            // {
            //     this.LoadCommonInfos(this.FilterCommonName);
            // }

            // private void LoadCommonInfos(string filterName = null)
            // {
            //     var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
            //     var UIConfig = AssetDatabaseExtension.GetOrCreateScriptableObject<UIConfig>(editorSetting.runtimeConfigPath);
            //     this.CommonInfos.Clear();
            //     if (string.IsNullOrEmpty(filterName))
            //     {
            //         this.CommonInfos.AddRange(UIConfig.CommonInfos);
            //     }
            //     else
            //     {
            //         filterName = filterName.ToLower();
            //         foreach (var commonInfo in UIConfig.CommonInfos)
            //         {
            //             if (commonInfo.ToLower().Contains(filterName))
            //             {
            //                 this.CommonInfos.Add(commonInfo);
            //             }
            //         }
            //     }
            //
            //     //根据深度排序
            //     this.CommonInfos.Sort(SortMethod);
            // }

            private int SortMethod(string a, string b)
            {
                return String.CompareOrdinal(a, b);
            }

            private string WidgetInfoDrawer(string value)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(value);

                #region 操作按钮

                if (EditorGUILayout.LinkButton("绑定"))
                {
                    Debug.Log($"{value} 绑定");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var success = TUIEditorHelper.BindUIVariables(editorSetting, prefab);
                    if (success)
                    {
                        AssetDatabase.Refresh();
                    }
                }

                if (EditorGUILayout.LinkButton("编辑"))
                {
                    Debug.Log($"{value} 编辑");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value, TUIEditorConst.UI_PREFAB_PATH);
                    if (prefab == null)
                        return value;
                    var scriptPath = TUIEditorHelper.GetUIScriptPath(editorSetting, prefab);
                    if (!string.IsNullOrEmpty(scriptPath))
                    {
                        InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 0);
                    }
                }

                if (EditorGUILayout.LinkButton("选中"))
                {
                    Debug.Log($"{value} 选中");
                    var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                    var prefab = AssetDatabaseExtension.LoadPrefabByName(value, TUIEditorConst.UI_PREFAB_PATH);
                    Selection.activeObject = prefab;
                }

                #endregion

                EditorGUILayout.EndHorizontal();
                return value;
            }
        }

        #endregion

        private GeneralSetting m_generalSetting = new GeneralSetting();
        private WindowsLayerSetting m_windowsLayerSetting = new WindowsLayerSetting();
        private WindowsSetting m_windowsSetting = new WindowsSetting();
        private WidgetsSetting m_widgetsSetting = new WidgetsSetting();
        private CommonsSetting m_commonsSetting = new CommonsSetting();

        //等待打开的预制体
        private static GameObject m_waitToOpenPrefab;

        private void Update()
        {
            if (m_waitToOpenPrefab != null)
            {
                PrefabStageUtility.OpenPrefab(AssetDatabase.GetAssetPath(m_waitToOpenPrefab));
                Selection.activeObject = m_waitToOpenPrefab;
                m_waitToOpenPrefab = null;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            tree.Add("通 用 设 置", m_generalSetting);
            tree.Add("层 级 管 理", m_windowsLayerSetting);
            tree.Add("窗 口 管 理", m_windowsSetting);
            tree.Add("控 件 管 理", m_widgetsSetting);
            tree.Add("组 件 管 理", m_commonsSetting);
            tree.Selection.SelectionChanged += (x) =>
            {
                if (x == SelectionChangedType.ItemAdded)
                {
                    if (tree.Selection.SelectedValue is GeneralSetting generalSetting)
                    {
                        generalSetting.Init();
                    }

                    if (tree.Selection.SelectedValue is WindowsLayerSetting m_windowsLayerSetting)
                    {
                        m_windowsLayerSetting.Init();
                    }

                    if (tree.Selection.SelectedValue is WindowsSetting windowsSetting)
                    {
                        windowsSetting.Init();
                    }

                    if (tree.Selection.SelectedValue is WidgetsSetting widegtsSetting)
                    {
                        widegtsSetting.Init();
                    }

                    // if (tree.Selection.SelectedValue is CommonsSetting m_commonsSetting)
                    // {
                    //     m_commonsSetting.Init();
                    // }
                }
            };
            return tree;
        }
    }
}