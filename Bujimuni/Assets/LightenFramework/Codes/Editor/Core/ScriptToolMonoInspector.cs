
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Lighten.Editor
{
    [CustomEditor(typeof(ScriptToolMono))]
    public class ScriptToolMonoInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var obj = target as ScriptToolMono;
            if (obj == null)
                return;
            var sourceGameObject = AssetDatabaseExtension.GetSourceGameObject(obj.gameObject);
            if (string.IsNullOrEmpty(obj.ClassName))
            {
                obj.ClassName = sourceGameObject.name;
            }
            
            obj.ClassName = EditorGUILayout.TextField("脚本名称", obj.ClassName);
            
            GUI.color = Color.green;
            if (GUILayout.Button("编辑", GUILayout.Height(40)))
            {
                if (GoScriptGenerator.Instance.AddControllerToGameObject(sourceGameObject.name, sourceGameObject))
                {
                    AssetDatabase.Refresh();
                }
                var scriptDir = GoScriptGenerator.Instance.GetScriptDir(sourceGameObject, GoScriptGenerator.OUTPUT_FOLDER_NAME);
                var filePath = $"{scriptDir}/{sourceGameObject.name}Controller.cs";
                if (!File.Exists(filePath))
                {
                    EditorUtility.DisplayDialog("提示", $"{filePath}不存在", "OK");
                    return;
                }
                InternalEditorUtility.OpenFileAtLineExternal(filePath, 0);
            }
        }
    }
}
