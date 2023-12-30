using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Lighten.Editor
{
    public abstract class ScriptGeneratorBase
    {
        private const string prefixOfIgnoreAutoBind = "i";

        public enum EFilterType
        {
            None = 0,
            OnlyChildren,
            SelfNotChildren,
            SelfAndChildren,
        }

        //构造函数
        public ScriptGeneratorBase()
        {
            this.RegisterGenerateMethod("g", this.GenerateGameObjectDefineCode);
            this.RegisterGenerateMethod("w", this.GenerateComponentsDefineCode);
        }

        //是否是嵌套节点
        protected abstract bool IsNestNode(Transform node);

        //创建嵌套节点代码
        protected abstract (string, string) CreateNestNodeCode(Transform node);

        //是否无视的自身组件
        protected abstract bool IsSelfIgnoreComponent(Component component);

        private Dictionary<string, Func<Transform, (string, string)>> m_generateMethods =
            new Dictionary<string, Func<Transform, (string, string)>>();

        protected void RegisterGenerateMethod(string prefixName, Func<Transform, (string, string)> func)
        {
            m_generateMethods[prefixName] = func;
        }

        protected bool HasGenerateMethod(string prefixName)
        {
            return m_generateMethods.ContainsKey(prefixName);
        }

        protected List<Transform> CollectNodes(Transform root)
        {
            var nodes = new List<Transform>();
            nodes.Add(root);
            CollectNodes(ref nodes, root);
            return nodes;
        }

        //收集符合条件的节点
        private void CollectNodes(ref List<Transform> nodes, Transform root)
        {
            foreach (Transform child in root)
            {
                var r = FilterNode(child);
                if (r == EFilterType.None)
                {
                    continue;
                }

                if (r == EFilterType.SelfNotChildren)
                {
                    nodes.Add(child);
                    continue;
                }

                if (r == EFilterType.SelfAndChildren)
                {
                    nodes.Add(child);
                }

                if (child.childCount > 0)
                {
                    CollectNodes(ref nodes, child);
                }
            }
        }

        //过滤节点
        protected EFilterType FilterNode(Transform node)
        {
            var prefixName = GetPrefixName(node.name);
            if (prefixName == prefixOfIgnoreAutoBind)
            {
                return EFilterType.None;
            }

            if (this.IsNestNode(node))
            {
                return EFilterType.SelfNotChildren;
            }

            if (HasGenerateMethod(prefixName))
            {
                return EFilterType.SelfAndChildren;
            }

            return EFilterType.OnlyChildren;
        }

        //生成变量绑定代码
        protected (string, string) GenerateVariablesCode(Transform root, List<Transform> nodes)
        {
            var defineCode = string.Empty;
            var removeCode = string.Empty;

            defineCode += "#region 自动生成变量定义\n";
            removeCode += "#region 自动生成变量销毁\n";

            foreach (var node in nodes)
            {
                if (node == root)
                {
                    var result = GenerateSelfComponentsDefineCode(node);
                    defineCode += result.Item1 + "\n";
                    removeCode += result.Item2 + "\n";
                    continue;
                }

                if (IsNestNode(node))
                {
                    var result = CreateNestNodeCode(node);
                    defineCode += result.Item1 + "\n";
                    removeCode += result.Item2 + "\n";
                    continue;
                }

                var prefix = GetPrefixName(node.name);
                if (this.m_generateMethods.TryGetValue(prefix, out var method))
                {
                    var result = method(node);
                    defineCode += result.Item1 + "\n";
                    removeCode += result.Item2 + "\n";
                }
            }

            defineCode += "#endregion";
            removeCode += "#endregion";

            return (defineCode, removeCode);
        }

        //生成变量绑定代码 前缀:"g_"
        protected (string, string) GenerateGameObjectDefineCode(Transform node)
        {
            var variableName = node.name.Substring(2) + "GameObject";
            var defineCode = $"private GameObject m_{variableName};\n" +
                             $"public GameObject {variableName}\n" +
                             "{\n" +
                             "\tget\n" +
                             "\t{\n" +
                             "\t\tif (Transform == null)\n" +
                             "\t\t{\n" +
                             "\t\t\tDebug.LogError(\"Transform is null\");\n" +
                             "\t\t\treturn null;\n" +
                             "\t\t}\n" +
                             $"\t\tif (m_{variableName} == null)\n" +
                             "\t\t{\n" +
                             $"\t\t\tm_{variableName} = Transform.Q(\"{node.name}\").gameObject;\n" +
                             "\t\t}\n" +
                             $"\t\treturn m_{variableName};\n" +
                             "\t}\n" +
                             "}\n";
            var removeCode = $"m_{variableName} = null;";
            return (defineCode, removeCode);
        }

        //生成变量绑定代码 前缀:"w_"
        protected (string, string) GenerateComponentsDefineCode(Transform node)
        {
            var defineCode = string.Empty;
            var removeCode = string.Empty;
            var result = GenerateGameObjectDefineCode(node);
            defineCode += result.Item1 + "\n";
            removeCode += result.Item2 + "\n";
            var components = node.GetComponents<Component>();
            foreach (var component in components)
            {
                var r = GenerateComponentDefineCode(node, component);
                defineCode += r.Item1 + "\n";
                removeCode += r.Item2 + "\n";
            }

            return (defineCode, removeCode);
        }

        //生成变量绑定代码 自身脚本
        protected (string, string) GenerateSelfComponentsDefineCode(Transform node)
        {
            var defineCode = string.Empty;
            var removeCode = string.Empty;
            var components = node.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component is Transform)
                    continue;
                if (IsSelfIgnoreComponent(component))
                    continue;
                var componentType = component.GetType();
                var variableName = "Self" + componentType.Name;
                var r = GenerateSelfComponentDefineCode(variableName, componentType);
                defineCode += r.Item1 + "\n";
                removeCode += r.Item2 + "\n";
            }

            return (defineCode, removeCode);
        }

        //生成变量绑定代码
        protected (string, string) GenerateComponentDefineCode(Transform node, Component component)
        {
            var componentType = component.GetType();
            var variableName = node.name.Substring(2) + componentType.Name;
            return GenerateComponentDefineCode(variableName, componentType, node.name);
        }

        protected (string, string) GenerateComponentDefineCode(string variableName, Type componentType, string nodeName)
        {
            var defineCode = $"private {componentType.FullName} m_{variableName};\n" +
                             $"public {componentType.FullName} {variableName}\n" +
                             "{\n" +
                             "\tget\n" +
                             "\t{\n" +
                             "\t\tif (Transform == null)\n" +
                             "\t\t{\n" +
                             "\t\t\tDebug.LogError(\"Transform is null\");\n" +
                             "\t\t\treturn null;\n" +
                             "\t\t}\n" +
                             $"\t\tif (m_{variableName} == null)\n" +
                             "\t\t{\n" +
                             $"\t\t\tm_{variableName} = Transform.Q<{componentType.FullName}>(\"{nodeName}\");\n" +
                             "\t\t}\n" +
                             $"\t\treturn m_{variableName};\n" +
                             "\t}\n" +
                             "}\n";
            var removeCode = $"m_{variableName} = null;";
            return (defineCode, removeCode);
        }

        protected (string, string) GenerateSelfComponentDefineCode(string variableName, Type componentType)
        {
            var defineCode = $"private {componentType.FullName} m_{variableName};\n" +
                             $"public {componentType.FullName} {variableName}\n" +
                             "{\n" +
                             "\tget\n" +
                             "\t{\n" +
                             "\t\tif (Transform == null)\n" +
                             "\t\t{\n" +
                             "\t\t\tDebug.LogError(\"Transform is null\");\n" +
                             "\t\t\treturn null;\n" +
                             "\t\t}\n" +
                             $"\t\tif (m_{variableName} == null)\n" +
                             "\t\t{\n" +
                             $"\t\t\tm_{variableName} = Transform.GetComponent<{componentType.FullName}>();\n" +
                             "\t\t}\n" +
                             $"\t\treturn m_{variableName};\n" +
                             "\t}\n" +
                             "}\n";
            var removeCode = $"m_{variableName} = null;";
            return (defineCode, removeCode);
        }

        //缩进代码
        protected string IndentCode(string code, int indent)
        {
            var lines = code.Split('\n');
            var indentStr = new string('\t', indent);
            var result = "";
            foreach (var line in lines)
            {
                result += indentStr + line + "\n";
            }

            return result;
        }

        //获取名称前缀
        protected string GetPrefixName(string name)
        {
            var index = name.IndexOf('_');
            if (index == -1)
            {
                return string.Empty;
            }

            return name.Substring(0, index);
        }

        //
        protected Type GetType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                !assembly.FullName.StartsWith("Unity"));

            var type = assemblies.Where(a => a.GetType(typeName) != null)
                .Select(a => a.GetType(typeName)).FirstOrDefault();

            return type;
        }
        
        public string GetScriptDir(GameObject gameObject, string folderName)
        {
            var scriptDir = "Assets/";
            var prefabAssetType = PrefabUtility.GetPrefabAssetType(gameObject);
            switch (prefabAssetType)
            {
                case PrefabAssetType.Regular:
                case PrefabAssetType.Model:
                case PrefabAssetType.Variant:
                {
                    var prefab = gameObject;
                    if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
                    {
                        prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
                    }

                    var assetPath = AssetDatabase.GetAssetPath(prefab);
                    var result = AssetDatabaseExtension.SearchFolderInParent(assetPath, folderName);
                    if (!string.IsNullOrEmpty(result))
                    {
                        scriptDir = result;
                    }
                }
                    break;
                case PrefabAssetType.NotAPrefab:
                {
                    string assetPath;
                    if (EditorSceneManager.IsPreviewSceneObject(gameObject))
                    {
                        var prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
                        assetPath = prefabStage.assetPath;
                    }
                    else
                    {
                        assetPath = gameObject.scene.path;
                    }

                    Debug.Log($"当前选中的Prefab路径为 {assetPath}");
                    var result = AssetDatabaseExtension.SearchFolderInParent(assetPath, folderName);
                    if (!string.IsNullOrEmpty(result))
                    {
                        scriptDir = result;
                    }
                }
                    break;
            }

            return scriptDir;
        }
        
    }
}