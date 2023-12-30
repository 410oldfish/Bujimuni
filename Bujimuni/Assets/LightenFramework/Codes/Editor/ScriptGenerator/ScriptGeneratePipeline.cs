using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lighten.Editor
{
    public class ScriptGeneratePipeline : ScriptableObject
    {
        private const string Dir = "Assets/LightenFramework/Codes/Editor/ScriptGenerator/";
        private const string FileName = "Pipeline.asset";
        
        private static ScriptGeneratePipeline m_instance;
        
        public static ScriptGeneratePipeline Default
        {
            get
            {
                if (m_instance != null)
                {
                    return m_instance;
                }
                var filePath = Dir + FileName;

                if (File.Exists(filePath))
                {
                    return m_instance = AssetDatabase.LoadAssetAtPath<ScriptGeneratePipeline>(filePath);
                }
                return m_instance = CreateInstance<ScriptGeneratePipeline>();
            }
        }

        public void Save()
        {
            var filePath = Dir + FileName;

            if (!File.Exists(filePath))
            {
                AssetDatabase.CreateAsset(this, filePath);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [System.Serializable]
        public class TaskData
        {
            public GameObject GameObject;
            public string ClassName;
        }

        public List<TaskData> CurrentTasks = new List<TaskData>();
        
        public void AddTask(GameObject gameObject, string className)
        {
            var taskData = new TaskData
            {
                GameObject = gameObject,
                ClassName = className
            };
            CurrentTasks.Add(taskData);
        }
    }
}
