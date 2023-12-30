using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace Lighten.Editor
{
    [CustomEditor(typeof(LongPressButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Button Component.
    ///   Extend this class to write a custom editor for a component derived from Button.
    /// </summary>
    public class LongPressButtonEditor : SelectableEditor
    {
        SerializedProperty m_OnClickProperty;
        SerializedProperty m_OnLongPressProperty;
        
        SerializedProperty m_longPressTriggerTimeProperty;
        SerializedProperty m_longPressIntervalTimeProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
            m_OnLongPressProperty = serializedObject.FindProperty("m_onLongPress");
            
            m_longPressTriggerTimeProperty = serializedObject.FindProperty("longPressTriggerTime");
            m_longPressIntervalTimeProperty = serializedObject.FindProperty("longPressIntervalTime");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_OnClickProperty);
            EditorGUILayout.PropertyField(m_OnLongPressProperty);

            EditorGUILayout.PropertyField(m_longPressTriggerTimeProperty);
            EditorGUILayout.PropertyField(m_longPressIntervalTimeProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }

}
