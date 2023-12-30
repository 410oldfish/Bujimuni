using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;

#endif

namespace Lighten
{
    public class XMonoVariables : MonoBehaviour
    {
        [CustomContextMenu("打印数据", "CheckValue")] public List<XMonoVariable> variables = new List<XMonoVariable>();

        private Dictionary<string, XMonoVariable> m_dicVariables = new Dictionary<string, XMonoVariable>();
        private bool m_initialized;
        private void Awake()
        {
            Initialize();
        }

        #region 取值

        public void Initialize()
        {
            if (m_initialized)
                return;
            foreach (var variable in variables)
            {
                if (m_dicVariables.ContainsKey(variable.name))
                    continue;
                m_dicVariables.Add(variable.name, variable);
            }
            m_initialized = true;
        }

        public XMonoVariable GetVariable(string name)
        {
            if (m_dicVariables.ContainsKey(name))
                return m_dicVariables[name];
            Debug.LogError($"{transform.name} 没有找到变量 {name}");
            return null;
        }

        public T GetValue<T>(string name) where T : class
        {
            var variable = GetVariable(name);
            if (variable == null || (variable.type != XMonoVariableType.Component && variable.type != XMonoVariableType.GameObject))
                return null;
            return variable.objData as T;
        }
        
        public T GetValueAsComponent<T>(string name) where T : Component
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Component)
                return null;
            return variable.objData as T;
        }

        public GameObject GetValueAsGameObject(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.GameObject)
                return null;
            return variable.objData as GameObject;
        }

        public bool GetValueAsBool(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Bool)
                return false;
            return XMonoVariableUtility.ToBool(variable.valData);
        }

        public float GetValueAsFloat(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Float)
                return 0f;
            return XMonoVariableUtility.ToSingle(variable.valData);
        }

        public int GetValueAsInt(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Int)
                return 0;
            return XMonoVariableUtility.ToInt32(variable.valData);
        }

        public string GetValueAsString(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.String)
                return string.Empty;
            return variable.valData;
        }

        public Color GetValueAsColor(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Color)
                return Color.white;
            return XMonoVariableUtility.ToColor(variable.valData);
        }

        public Vector2 GetValueAsVector2(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Vector2)
                return Vector2.zero;
            return XMonoVariableUtility.ToVector2(variable.valData);
        }

        public Vector2 GetValueAsVector3(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Vector3)
                return Vector3.zero;
            return XMonoVariableUtility.ToVector3(variable.valData);
        }

        public Vector2 GetValueAsVector4(string name)
        {
            var variable = GetVariable(name);
            if (variable == null || variable.type != XMonoVariableType.Vector4)
                return Vector4.zero;
            return XMonoVariableUtility.ToVector4(variable.valData);
        }

        #endregion
        
        public void RecordNodePath()
        {
#if UNITY_EDITOR
            Record();
#endif
        }

#if UNITY_EDITOR
        [Button("记录引用", ButtonSizes.Large), HorizontalGroup()]
        void Record()
        {
            foreach (var value in variables)
            {
                if (value.type != XMonoVariableType.GameObject &&
                    value.type != XMonoVariableType.Component)
                {
                    continue;
                }
                if (gameObject == value.gameobject)
                {
                    value.isself = true;
                    value.recorded = true;
                    Debug.Log($"{value.name} = 自己");
                    continue;
                }
                var relativePath = XMonoVariableUtility.GetChildPath(transform, value.gameobject.transform);
                if (string.IsNullOrEmpty(relativePath))
                {
                    Debug.LogError($"{value.name} 引用了不是自己Child的节点！！！！！！！");
                    value.recorded = false;
                    continue;
                }
                value.relativePath = relativePath;
                value.recorded = true;
                Debug.Log($"{value.name} = {value.relativePath}");
            }
        }

        [Button("恢复引用", ButtonSizes.Large), HorizontalGroup()]
        void Recover()
        {
            foreach (var value in variables)
            {
                if (value.type != XMonoVariableType.GameObject &&
                    value.type != XMonoVariableType.Component)
                {
                    continue;
                }
                if (value.isself)
                {
                    value.gameobject = gameObject;
                    continue;
                }
                if (string.IsNullOrEmpty(value.relativePath))
                {
                    Debug.LogError($"{value.name} 没有记录路径");
                }
                else
                {
                    var child = transform.Find(value.relativePath);
                    if (child == null)
                    {
                        Debug.LogError($"{value.relativePath} 不存在");
                        continue;
                    }
                    value.gameobject = child.gameObject;
                }
            }
        }

        void CheckValue()
        {
            var text = $"===打印数据=== count:{variables.Count}\n";
            foreach (var value in variables)
            {
                text += $"{value.name}:{value.GetValue()}";
            }
            Debug.Log(text);
        }

        public bool Exist(string name)
        {
            foreach (var val in variables)
            {
                if (val.name == name)
                    return true;
            }
            return false;
        }
#endif
    }
}