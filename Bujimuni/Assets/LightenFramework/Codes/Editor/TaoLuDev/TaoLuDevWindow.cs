using System;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Lighten.Editor
{
    public class TaoLuDevWindow: OdinMenuEditorWindow
    {
        //帮助按钮
        private void ShowButton(Rect position)
        {
            TaoLuDevHelper.DrawHelpWindow(position, "1.资源管理");
        }

        
        #region 资 源 管 理

        public class ResourcesSetting
        {
            public void Init()
            {
            }

            [HorizontalGroup("1")]
            [Button("生 成 luban", ButtonSizes.Large), GUIColor(0.85f, 0.85f, 0)]
            public void GenerateLuban()
            {
                var errorMsg = TaoLuDevHelper.GenerateLuban();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    EditorUtility.DisplayDialog("提示", errorMsg, "OK");
                    return;
                }
                TaoLuDevHelper.GenerateAddressableGroups();
                EditorUtility.DisplayDialog("提示", "生 成 luban完成!", "OK");
            }

            [HorizontalGroup("1")]
            [Button("生 成 精 灵", ButtonSizes.Large), GUIColor(0.85f, 0.85f, 0)]
            public void GenerateSpriteConfig()
            {
                TaoLuDevHelper.GenerateSpriteConfig();
                TaoLuDevHelper.GenerateAddressableGroups();
                EditorUtility.DisplayDialog("提示", "生 成 精 灵完成!", "OK");
            }

            [HorizontalGroup("2")]
            [Button("生 成 图 集(UI自动图集)", ButtonSizes.Large), GUIColor(0.85f, 0.85f, 0)]
            public void GenerateUIAtlas()
            {
                TaoLuDevHelper.GenerateUIAtlas();
                TaoLuDevHelper.GenerateAddressableGroups();
                EditorUtility.DisplayDialog("提示", "生 成 图 集完成!", "OK");
            }

            [HorizontalGroup("2")]
            [Button("生 成 AddressableGroups", ButtonSizes.Large), GUIColor(0.85f, 0.85f, 0)]
            public void GenerateAddressableGroups()
            {
                TaoLuDevHelper.GenerateAddressableGroups();
                EditorUtility.DisplayDialog("提示", "生 成 AddressableGroups完成!", "OK");
            }

            [Button("一 键 所 有", ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
            public void GenerateAll()
            {
                var errorMsg = string.Empty;
                errorMsg = TaoLuDevHelper.GenerateLuban();
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    EditorUtility.DisplayDialog("提示", errorMsg, "OK");
                    return;
                }

                TaoLuDevHelper.GenerateSpriteConfig();
                TaoLuDevHelper.GenerateUIAtlas();
                TaoLuDevHelper.GenerateAddressableGroups();
                EditorUtility.DisplayDialog("提示", "一条龙完成!", "OK");
            }

            [Button("打 包 热 更 资 源", ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
            public void BuildHotfixRes()
            {
                //获取git版本号
                // var versionControl = new VersionControl();
                // versionControl.ResVer = GitTool.GetCurrentGitCommitId();
                // versionControl.ResBuildDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                // File.WriteAllText("Assets/Bundles/Config/ConfigOfText/VersionControl.txt", JsonConvert.SerializeObject(versionControl));

                //代码编译
                //BuildAssemblieEditor.BuildModelAndHotfix(false);
                //TODO:目前只打QA服
                //BuildEditor.HotPackBundleForQA();
                //复制到指定目录
                var sourceDir = $"{Application.dataPath}/../ServerDataQA/latestRes/";
                var targetDir = $"{Application.dataPath}/../Remote/";
                if (Directory.Exists(targetDir))
                {
                    Directory.Delete(targetDir, true);
                }

                FileUtil.CopyFileOrDirectory(sourceDir, targetDir);
                EditorUtility.DisplayDialog("提示", "热 更 资 源 完 成!", "OK");
            }
        }

        #endregion

        private ResourcesSetting m_ResourcesSetting = new ResourcesSetting();
        //private WindowsSetting m_windowsSetting = new WindowsSetting();
        //private WidgetsSetting m_widgetsSetting = new WidgetsSetting();
        //private CommonsSetting m_commonsSetting = new CommonsSetting();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Selection.SupportsMultiSelect = false;
            tree.Add("资 源 管 理", m_ResourcesSetting);
            // tree.Add("窗 口 管 理", m_windowsSetting);
            // tree.Add("控 件 管 理", m_widgetsSetting);
            // tree.Add("组 件 管 理", m_commonsSetting);
            tree.Selection.SelectionChanged += (x) =>
            {
                if (x == SelectionChangedType.ItemAdded)
                {
                    if (tree.Selection.SelectedValue is ResourcesSetting setting)
                    {
                        setting.Init();
                    }
                    // if (tree.Selection.SelectedValue is WindowsSetting windowsSetting)
                    // {
                    //     windowsSetting.Init();
                    // }
                    // if (tree.Selection.SelectedValue is WidgetsSetting widegtsSetting)
                    // {
                    //     widegtsSetting.Init();
                    // }
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