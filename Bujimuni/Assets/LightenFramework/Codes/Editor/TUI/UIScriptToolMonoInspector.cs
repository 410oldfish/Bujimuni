using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Lighten.Editor
{
    [CustomEditor(typeof(UIScriptToolMono))]
    public class UIScriptToolMonoInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var obj = target as UIScriptToolMono;
            if (obj == null)
                return;
            if (string.IsNullOrEmpty(obj.ClassName))
            {
                obj.ClassName = obj.gameObject.name;
            }
            obj.ClassName = EditorGUILayout.TextField("脚本名称", obj.ClassName);
            
            GUI.color = Color.green;
            if (GUILayout.Button("编辑", GUILayout.Height(40)))
            {
                var selectedGameObject = Selection.activeGameObject;
                if (selectedGameObject == null || !TUIEditorHelper.IsValidUIName(selectedGameObject))
                    return;
                var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(selectedGameObject);
                var editorSetting = TUIEditorHelper.GetTUIEditorSetting();
                if (TUIEditorHelper.IsNeedToUpdateUIConfig(editorSetting, sourceGameObject))
                {
                    TUIEditorHelper.GenerateUIConfig();
                }
                var success = TUIEditorHelper.BindUIVariables(editorSetting, sourceGameObject);
                if (success)
                {
                    AssetDatabase.Refresh();
                }
                var scriptPath = TUIEditorHelper.GetUIScriptPath(editorSetting, sourceGameObject);
                if (!string.IsNullOrEmpty(scriptPath))
                {
                    InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 0);
                }
            }
        }
    }
}