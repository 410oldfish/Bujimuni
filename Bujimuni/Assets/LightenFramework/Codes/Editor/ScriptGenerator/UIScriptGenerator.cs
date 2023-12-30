using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Lighten.Editor
{
    //UI代码生成器
    public class UIScriptGenerator : ScriptGeneratorBase
    {
        public static UIScriptGenerator Instance { get; private set; } = new UIScriptGenerator();

        //
        public void Generate(GameObject gameObject, string templateName, string folderName)
        {
            var scriptDir = this.GetScriptDir(gameObject, folderName);
            //创建View
            GenerateGoViewScript(gameObject, templateName, scriptDir);
            //创建Ctrl
            GenerateGoCtrlScript(gameObject, templateName, scriptDir);
        }

        //生成GameObject的Contrller脚本(只会在首次生成)
        private void GenerateGoCtrlScript(GameObject gameObject, string templateName, string outputDir)
        {
            var ctrlName = gameObject.name + "";
            var filePath = $"{outputDir}/{ctrlName}.cs";
            if (File.Exists(filePath))
            {
                return;
            }

            var templateFilePath = Path.Combine(Application.dataPath,
                $"LightenFramework/Codes/Editor/ScriptGenerator/Templates/{templateName}.cs");
            var content = File.ReadAllText(templateFilePath);
            content = content.Replace($"{templateName} /*ScriptName*/", ctrlName);

            File.WriteAllText(filePath, content);
        }

        //生成GameObject的View脚本
        private void GenerateGoViewScript(GameObject gameObject, string templateName, string outputDir)
        {
            var ctrlName = gameObject.name + "";
            var filePath = $"{outputDir}/{ctrlName}.View.cs";
            var templateFilePath = Path.Combine(Application.dataPath,
                $"LightenFramework/Codes/Editor/ScriptGenerator/Templates/{templateName}.View.cs");
            var content = File.ReadAllText(templateFilePath);
            content = content.Replace($"{templateName} /*ScriptName*/", ctrlName);

            var autoBindNodes = CollectNodes(gameObject.transform);
            var result = GenerateVariablesCode(gameObject.transform, autoBindNodes);
            content = content.Replace("/*FIELDS_DEFINE*/", IndentCode(result.Item1, 2));
            content = content.Replace("/*FIELDS_REMOVE*/", IndentCode(result.Item2, 3));
            File.WriteAllText(filePath, content);
        }


        protected override bool IsNestNode(Transform node)
        {
            var prefixName = UIHelper.GetPrefixName(node.name);
            if (prefixName == UIPrefix.Window || prefixName == UIPrefix.Widget || prefixName == UIPrefix.OSA)
                return true;
            return false;
        }

        protected override (string, string) CreateNestNodeCode(Transform node)
        {
            var prefixName = UIHelper.GetPrefixName(node.name);
            var defineCode = string.Empty;
            var removeCode = string.Empty;
            switch (prefixName)
            {
                case UIPrefix.Widget:
                {
                    var source = AssetDatabaseExtension.GetSourceGameObject(node.gameObject);
                    return GenerateWidgetCode(node.name, source.name);
                }
                case UIPrefix.OSA:
                    return GenerateVariableDefineOfOSA(node);
            }

            return (defineCode, removeCode);
        }

        protected override bool IsSelfIgnoreComponent(Component component)
        {
            return false;
        }

        private (string, string) GenerateWidgetCode(string variableName, string widgetType)
        {
            var defineCode = $"private {widgetType} m_{variableName};\n" +
                             $"public {widgetType} {variableName}\n" +
                             "{\n" +
                             "\tget\n" +
                             "\t{\n" +
                             $"\t\tif (m_{variableName} == null)\n" +
                             "\t\t{\n" +
                             $"\t\t\tm_{variableName} = this.GetParent<UIWindow>().GetWidget<{widgetType}>(\"{variableName}\");\n" +
                             "\t\t}\n" +
                             $"\t\treturn m_{variableName};\n" +
                             "\t}\n" +
                             "}\n";
            var removeCode = $"m_{variableName} = null;";
            return (defineCode, removeCode);
        }

        private (string, string) GenerateVariableDefineOfOSA(Transform node)
        {
            var variableName = node.gameObject.name;
            var componentType = string.Empty;
            if (string.IsNullOrEmpty(componentType) && node.GetComponent<OSASimpleScrollView>() != null)
            {
                componentType = "Lighten.OSASimpleScrollViewComponent";
            }

            if (string.IsNullOrEmpty(componentType) && node.GetComponent<OSASimpleGridView>() != null)
            {
                componentType = "Lighten.OSASimpleGridViewComponent";
            }

            if (string.IsNullOrEmpty(componentType) && node.GetComponent<OSAMultiGroupGridView>() != null)
            {
                componentType = "Lighten.OSAMultiGroupGridViewComponent";
            }
            //var variablePath = root.GetChildPath(node);
            var defineCode = $"\t\tprivate {componentType} m_{variableName};\n" +
                             $"\t\tpublic {componentType} {variableName}\n" +
                             "\t\t{\n" +
                             "\t\t\tget\n" +
                             "\t\t\t{\n" +
                             $"\t\t\t\tif (m_{variableName} == null)\n" +
                             "\t\t\t\t{\n" +
                             $"\t\t\t\t\tm_{variableName} = this.GetParent<UIWidget>().GetChild<{componentType}>(\"{node.name}\");\n" +
                             "\t\t\t\t}\n" +
                             $"\t\t\t\treturn m_{variableName};\n" +
                             "\t\t\t}\n" +
                             "\t\t}\n";
            var removeCode = $"m_{variableName} = null;";
            return (defineCode, removeCode);
        }
    }
}