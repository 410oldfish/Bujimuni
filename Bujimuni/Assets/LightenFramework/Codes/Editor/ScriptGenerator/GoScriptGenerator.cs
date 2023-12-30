using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace Lighten.Editor
{
    //代码生成器
    public class GoScriptGenerator : ScriptGeneratorBase
    {
        public static GoScriptGenerator Instance { get; private set; } = new GoScriptGenerator();

        public const string OUTPUT_FOLDER_NAME = "Controllers";

        //给GameObject添加控制器
        public bool AddControllerToGameObject(string name, GameObject gameObject)
        {
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(gameObject);
            Debug.Log($"select is {prefabAssetType}");

            var scriptDir = this.GetScriptDir(gameObject, OUTPUT_FOLDER_NAME);
            
            //创建View
            GenerateGoViewScript(name, gameObject, scriptDir);
            
            //创建Ctrl
            var successed = GenerateGoCtrlScript(name, gameObject, scriptDir);
            if (successed)
            {
                //
                var handleGameObject = AssetDatabaseExtension.GetSourceGameObject(gameObject);
                ScriptGeneratePipeline.Default.AddTask(handleGameObject, name + "Controller");
                ScriptGeneratePipeline.Default.Save();
            
                if (!EditorApplication.isCompiling)
                {
                    OnCompileComplete();
                }

                return true;
            }

            return false;
        }
        
        protected void OnCompileComplete()
        {
            if (ScriptGeneratePipeline.Default.CurrentTasks.Count < 1)
                return;
            foreach (var taskData in ScriptGeneratePipeline.Default.CurrentTasks)
            {
                var type = this.GetType(taskData.ClassName);
                if (type == null || taskData.GameObject == null)
                    continue;
                var component = taskData.GameObject.GetComponent(type);
                if (component == null)
                {
                    component = taskData.GameObject.AddComponent(type);
                }
                EditorUtility.SetDirty(taskData.GameObject);
            }
            AssetDatabase.SaveAssets();
            ScriptGeneratePipeline.Default.CurrentTasks.Clear();
            ScriptGeneratePipeline.Default.Save();
        }

        [DidReloadScripts]
        static void CompileComplete()
        {
            Instance.OnCompileComplete();
        }
        
        //生成GameObject的Contrller脚本(只会在首次生成)
        protected bool GenerateGoCtrlScript(string name, GameObject gameObject, string outputDir)
        {
            var ctrlName = name + "Controller";
            var viewName = name + "View";
            var filePath = $"{outputDir}/{name}Controller.cs";
            if (File.Exists(filePath))
            {
                return false;
            }
            var templateFilePath = Path.Combine(Application.dataPath,
                "LightenFramework/Codes/Editor/ScriptGenerator/Templates/GameObjectScriptTemplate.Controller.cs");
            var content = File.ReadAllText(templateFilePath);
            content = content.Replace("GameObjectCtrlScriptTemplate /*ScriptName*/", ctrlName);
            content = content.Replace("GameObjectViewScriptTemplate /*ScriptName*/", viewName);

            File.WriteAllText(filePath, content);
            return true;
        }

        //生成GameObject的View脚本
        protected void GenerateGoViewScript(string name, GameObject gameObject, string outputDir)
        {
            var ctrlName = name + "Controller";
            var viewName = name + "View";
            var filePath = $"{outputDir}/{name}View.cs";
            var templateFilePath = Path.Combine(Application.dataPath,
                "LightenFramework/Codes/Editor/ScriptGenerator/Templates/GameObjectScriptTemplate.View.cs");
            var content = File.ReadAllText(templateFilePath);
            content = content.Replace("GameObjectCtrlScriptTemplate /*ScriptName*/", ctrlName);
            content = content.Replace("GameObjectViewScriptTemplate /*ScriptName*/", viewName);
            
            var autoBindNodes = CollectNodes(gameObject.transform);
            var result = GenerateVariablesCode(gameObject.transform, autoBindNodes);
            content = content.Replace("/*FIELDS_DEFINE*/", IndentCode(result.Item1, 2));
            content = content.Replace("/*FIELDS_REMOVE*/", IndentCode(result.Item2, 3));
            File.WriteAllText(filePath, content);
        }
        
        protected override bool IsNestNode(Transform node)
        {
            var component = node.GetComponent<XEntityController>();
            return component != null;
        }

        protected override (string, string) CreateNestNodeCode(Transform node)
        {
            var component = node.GetComponent<XEntityController>();
            var componentType = component.GetType();
            var variableName = node.name.Substring(2) + "Controller";
            return GenerateComponentDefineCode(variableName, componentType, node.name);
        }

        protected override bool IsSelfIgnoreComponent(Component component)
        {
            return component is XEntityController;
        }
    }
}