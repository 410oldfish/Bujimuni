using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;


[CustomEditor(typeof (StatController))]
public class StatControllerEditor: UnityEditor.Editor
{
    private StatController _ui;

    private ILuaExporter _luaExporter;

    private GUIStyle lableStyleRed = new GUIStyle();

    private GUIStyle lableStyleBlue = new GUIStyle();
    
    private const string OpenStatControllerWindowStr = "状态控制器窗口";

    private string openWindowStr = OpenStatControllerWindowStr;

    SerializedObject m_Object;

    private void OnEnable()
    {
        lableStyleRed.fontSize = 12;
        lableStyleRed.normal.textColor = new Color(0.8f, 0.3f, 0.3f, 1);

        lableStyleBlue.fontSize = 12;
        lableStyleBlue.normal.textColor = new Color(46f / 256f, 163f / 256f, 256f / 256f, 256f / 256f);


        m_Object = new SerializedObject(target);

        //RefreshLuaExporter(StatControllerEditorConfigSettingsProvider.GetConfig());
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        _ui = target as StatController;
        if (Application.isPlaying)
        {
            OnEditorModeGUI();
        }
        else
        {
            OnEditorModeGUI();
        }
    }

    private void OnEditorModeGUI()
    {
        openWindowStr = OpenStatControllerWindowStr;
        
        DisplayController();


        if (GUILayout.Button(openWindowStr))
        {
            StatControllerWindow window = EditorWindow.GetWindow<StatControllerWindow>();
            window.ChangeTarget(_ui);
        }

    }

    private void DisplayController()
    {
        #region Controller

        if (_ui.ControllerConfigs != null && _ui.ControllerConfigs.Length > 0)
        {
            EditorGUILayout.LabelField("Controllers:", EditorStyles.boldLabel);
            for (int i = 0; i < _ui.ControllerConfigs.Length; i++)
            {
                ControllerConfig config = _ui.ControllerConfigs[i];
                int[] optionValues = new int[config.pageNames.Length];

                for (int j = 0; j < config.pageNames.Length; j++)
                {
                    optionValues[j] = j;
                }

                GUIContent[] controllerOptionStrs = new GUIContent[config.pageNames.Length];
                for (int j = 0; j < config.pageNames.Length; j++)
                {
                    controllerOptionStrs[j] = new GUIContent();
                    controllerOptionStrs[j].text = string.Format("[{0}] {1}", j, config.pageNames[j]);
                    controllerOptionStrs[j].tooltip = config.pageTips[j];
                }

                GUIContent configName = new GUIContent("    " + config.name);

                int tempIndex = EditorGUILayout.IntPopup(configName, config.selectedIndex,
                    controllerOptionStrs, optionValues);
                if (tempIndex != config.selectedIndex)
                {
                    config.selectedIndex = tempIndex;
                    _ui.EditorChangeControllerSelectedIndex(config.name, tempIndex);
                    EditorUtility.SetDirty(_ui);
                }
            }
#if UNITY_2018
                for (int i = 0; i < 8; i++)
                {
                    EditorGUILayout.Space();
                }
#else
            EditorGUILayout.Space(8f);
#endif
        }

        #endregion
    }


    private bool HasOpenCloseAni()
    {
        if (_ui == null)
            return false;
        var go = _ui.gameObject;
        var animation = go.GetComponent<Animation>();
        if (animation == null)
            return false;
        if (animation.GetClip("close") == null && animation.GetClip("open") == null)
            return false;
        return true;
    }

    private void RemoveOpenCloseAnimation()
    {
        if (_ui == null)
            return;
        GameObject targetGo = _ui.gameObject;
        if (targetGo != null)
        {
            Animation animation = targetGo.GetComponent<Animation>();
            if (animation.GetClip("close") != null)
                animation.RemoveClip("close");
            if (animation.GetClip("open") != null)
                animation.RemoveClip("open");
            if (animation.GetClipCount() <= 0)
            {
                UnityEngine.Object.DestroyImmediate(animation, true);
            }
        }
    }

    private void RefreshOpenCloseAnimation(StatControllerEditorConfig editorConfig)
    {
        if (editorConfig.defaultCloseAni == null && editorConfig.defaultOpenAni == null)
            return;
        if (_ui == null)
            return;
        GameObject targetGo = _ui.gameObject;
        if (targetGo != null)
        {
            Animation animation = targetGo.GetComponent<Animation>();
            if (animation == null)
            {
                animation = targetGo.AddComponent<Animation>();
                if (editorConfig.defaultCloseAni != null)
                    animation.AddClip(editorConfig.defaultCloseAni, "close");
                if (editorConfig.defaultOpenAni != null)
                    animation.AddClip(editorConfig.defaultOpenAni, "open");
            }
            else
            {
                if (editorConfig.defaultCloseAni != null)
                {
                    if (animation.GetClip("close") != null)
                    {
                        animation.RemoveClip("close");
                    }

                    animation.AddClip(editorConfig.defaultCloseAni, "close");
                }

                if (editorConfig.defaultOpenAni != null)
                {
                    if (animation.GetClip("open"))
                    {
                        animation.RemoveClip("open");
                    }

                    animation.AddClip(editorConfig.defaultOpenAni, "open");
                }

            }
        }
    }

    private void OnPlayingModeGUI()
    {
        if (_ui.ControllerConfigs != null && _ui.ControllerConfigs.Length > 0)
        {
            EditorGUILayout.LabelField("Controllers:", EditorStyles.boldLabel);
            for (int i = 0; i < _ui.ControllerConfigs.Length; i++)
            {
                ControllerConfig config = _ui.ControllerConfigs[i];
                Controller controller = _ui.GetController(i);
                if (controller == null)
                {
                    continue;
                }

                int[] optionValues = new int[config.pageNames.Length];
                for (int j = 0; j < config.pageNames.Length; j++)
                {
                    optionValues[j] = j;
                }

                string[] controllerOptionStrs = new string[config.pageNames.Length];
                for (int j = 0; j < config.pageNames.Length; j++)
                {
                    controllerOptionStrs[j] = string.Format("[{0}] {1}", j, config.pageNames[j]);
                }

                int tempIndex = EditorGUILayout.IntPopup(config.name, controller.SelectedIndex,
                    controllerOptionStrs, optionValues);
                controller.SelectedIndex = tempIndex;
            }
#if UNITY_2018
                for (int i = 0; i < 8; i++)
                {
                    EditorGUILayout.Space();
                }
#else
            EditorGUILayout.Space(8f);
#endif
        }
    }
}