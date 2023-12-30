using UnityEditor;
using UnityEditorInternal;

namespace Lighten.Editor
{
    public static class TUIEditorMenu
    {
        [MenuItem(LightenEditorConst.MENU_ROOT + "/界面管理", priority = 10)]
        static void OpenWindow()
        {
            var window = EditorWindow.GetWindow<TUIEditorWindow>("界面管理");
            window.Show();
        }

        // [MenuItem("Assets/UI 快 捷 选 项/生成UI代码", priority = -100)]
        // [MenuItem("GameObject/UI 快 捷 选 项/生成UI代码 &9", priority = -100)]
        // static void GenerateWindowScriptInGameObjectMenu()
        // {
        //     var selectedGameObject = Selection.activeGameObject;
        //     if (selectedGameObject == null || !TUIEditorHelper.IsValidUIName(selectedGameObject))
        //         return;
        //     var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(selectedGameObject);
        //     var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
        //     if (TUIEditorHelper.IsNeedToUpdateUIConfig(editorSetting, sourceGameObject))
        //     {
        //         TUIEditorHelper.GenerateUIConfig();
        //     }
        //     var success = TUIEditorHelper.BindUIVariables(editorSetting, sourceGameObject);
        //     if (success)
        //     {
        //         AssetDatabase.Refresh();
        //     }
        // }
        //
        // [MenuItem("Assets/UI 快 捷 选 项/编辑UI代码", priority = -99)]
        // [MenuItem("GameObject/UI 快 捷 选 项/编辑UI代码 &0", priority = -99)]
        // static void OpenWindowScriptInGameObjectMenu()
        // {
        //     var selectedGameObject = Selection.activeGameObject;
        //     if (selectedGameObject == null || !TUIEditorHelper.IsValidUIName(selectedGameObject))
        //         return;
        //     var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(selectedGameObject);
        //     var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
        //     var scriptPath = TUIEditorHelper.GetUIScriptPath(editorSetting, sourceGameObject);
        //     if (!string.IsNullOrEmpty(scriptPath))
        //     {
        //         InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 0);
        //     }
        // }
    }
}