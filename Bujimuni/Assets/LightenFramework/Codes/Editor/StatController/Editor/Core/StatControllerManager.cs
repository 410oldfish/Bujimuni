using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class StatControllerManager
{
    private static StatControllerManager _instance;

    public static StatControllerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StatControllerManager();
            }

            return _instance;
        }
    }

    public StatControllerControllerPanelSettings ControllerSettings
    {
        get
        {
            return _windowSettings.ControllerSettings;
        }
    }

    public StatControllerControllerWrapper CurControllerWrapper
    {
        get
        {
            if (ControllerSettings.CurControllerIndex < 0)
            {
                return null;
            }

            return _curStatControllerWrapper.ControllerWrapperList[ControllerSettings.CurControllerIndex];
        }
    }

    private StatController _curStatController;

    private StatControllerWrapper _curStatControllerWrapper;

    private StatControllerWindowSettings _windowSettings;

    private double _accumulatedTime;

    private DateTime _previousTime;

    private bool _needRepaint;

    private bool _needReInit;

    private bool _autoSave;

    private bool m_isWindowOpen = false;

    public StatController CurStatController
    {
        get
        {
            return _curStatController;
        }
        set
        {
            if (_curStatController == value)
            {
                return;
            }

            SaveCurStatController(value);
            _curStatController = value;
            Init();
        }
    }

    public bool AutoSave
    {
        get
        {
            return _autoSave;
        }

        set
        {
            _autoSave = value;
        }
    }

    public StatControllerWrapper CurStatControllerWrapper
    {
        get
        {
            return _curStatControllerWrapper;
        }

        set
        {
            _curStatControllerWrapper = value;
        }
    }

    public StatControllerWindowSettings WindowSettings
    {
        get
        {
            return _windowSettings;
        }

        set
        {
            _windowSettings = value;
        }
    }

    public bool NeedRepaint
    {
        get
        {
            return _needRepaint;
        }

        set
        {
            _needRepaint = value;
        }
    }

    public bool IsWindowOpen
    {
        get
        {
            return m_isWindowOpen;
        }
        set
        {
            m_isWindowOpen = value;
        }
    }

    public bool NeedReInit
    {
        get => _needReInit;
        set => _needReInit = value;
    }

    private void SaveCurStatController(StatController statController)
    {
        if (statController == null)
        {
            EditorPrefs.SetInt("StatController.StatControllerGameObjectType", (int)StatControllerGameObjectType.None);
        }
        else
        {
            if (PrefabUtility.GetPrefabInstanceStatus(statController.gameObject) == PrefabInstanceStatus.Connected)
            {
                Debug.Log("是一个prefab的Instance");
                UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(statController.gameObject);
                string path = AssetDatabase.GetAssetPath(parentObject);
                EditorPrefs.SetInt("StatController.StatControllerGameObjectType", (int)StatControllerGameObjectType.PrefabInstance);
                EditorPrefs.SetInt("StatController.StatControllerGameObjectInstanceID", statController.gameObject.GetInstanceID());
            }
            else if (PrefabUtility.GetPrefabAssetType(statController.gameObject) == PrefabAssetType.Regular ||
                     PrefabUtility.GetPrefabAssetType(statController.gameObject) == PrefabAssetType.Variant)
            {
                Debug.Log("是一个prefab的本体");
                string path = AssetDatabase.GetAssetPath(statController.gameObject);
                EditorPrefs.SetInt("StatController.StatControllerGameObjectType", (int)StatControllerGameObjectType.PrefabAsset);
                EditorPrefs.SetString("StatController.StatControllerGameObjectGUID", AssetDatabase.AssetPathToGUID(path));
            }
            else
            {
                Debug.Log("是一个普通的GameObject");
                EditorPrefs.SetInt("StatController.StatControllerGameObjectType", (int)StatControllerGameObjectType.NormalGameObject);
                EditorPrefs.SetInt("StatController.StatControllerGameObjectInstanceID", statController.gameObject.GetInstanceID());
            }

        }
    }

    public void LoadLastStatController()
    {
        if (EditorPrefs.HasKey("StatController.StatControllerGameObjectType"))
        {
            StatControllerGameObjectType type = (StatControllerGameObjectType)EditorPrefs.GetInt("StatController.StatControllerGameObjectType");
            if (type == StatControllerGameObjectType.PrefabInstance ||
                type == StatControllerGameObjectType.NormalGameObject)
            {
                if (EditorPrefs.HasKey("StatController.StatControllerGameObjectInstanceID"))
                {
                    int targetInstanceID = EditorPrefs.GetInt("StatController.StatControllerGameObjectInstanceID");
                    GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof (GameObject));
                    foreach (GameObject go in gos)
                    {
                        if (go.gameObject.GetInstanceID() == 42860)
                        {
                            if (go.GetComponent<StatController>() != null)
                            {
                                // Debug.Log(go.name);
                            }
                        }

                        if (go.gameObject.GetInstanceID() == targetInstanceID && go.GetComponent<StatController>() != null)
                        {
                            Debug.Log(go.name);
                            _curStatController = go.GetComponent<StatController>();
                            Init();
                        }
                    }
                }
            }
            else if (type == StatControllerGameObjectType.PrefabAsset)
            {
                if (EditorPrefs.HasKey("StatController.StatControllerGameObjectGUID"))
                {
                    string path = AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString("StatController.StatControllerGameObjectGUID"));
                    if (!string.IsNullOrEmpty(path))
                    {
                        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                        if (go != null && go.GetComponent<StatController>() != null)
                        {
                            _curStatController = go.GetComponent<StatController>();
                            Init();
                        }
                    }
                }
            }
        }
    }

    public void Init()
    {
        _previousTime = DateTime.Now;
        _accumulatedTime = 0;
        if (_curStatController != null)
        {
            _curStatControllerWrapper = new StatControllerWrapper(_curStatController);
            _windowSettings = new StatControllerWindowSettings();
        }
        else
        {
            _curStatControllerWrapper = null;
            _windowSettings = null;
        }

    }

    public void OnWindowUpdate()
    {
        if (_curStatController == null)
        {
            return;
        }

        double delta = (DateTime.Now - _previousTime).TotalSeconds;
        _previousTime = DateTime.Now;
        if (delta <= 0)
        {
            return;
        }

        _accumulatedTime += delta;
        if (_accumulatedTime > StatControllerEditorUtility.WINDOW_REPAINT_TIME)
        {
            _needRepaint = true;
            _accumulatedTime -= StatControllerEditorUtility.WINDOW_REPAINT_TIME;
        }

        OnUpdate((float)delta);
    }

    private void OnUpdate(float deltaTime)
    {
        _windowSettings.Panels[_windowSettings.CurPanelIndex].OnUpdate(deltaTime);
    }

    public void OnDispose()
    {
        _instance = null;
    }

    #region Controller Action

    public void CreateNewControllerWrapper(string wrapperName, List<string> pageNameList, List<string> pageTipsList)
    {
        StatControllerControllerWrapper controllerWrapper = new StatControllerControllerWrapper(wrapperName, pageNameList, pageTipsList);
        _curStatControllerWrapper.ControllerWrapperList.Add(controllerWrapper);
        ControllerSettings.CurControllerIndex = _curStatControllerWrapper.ControllerWrapperList.Count - 1;
    }

    public void DeleteCurControllerWrapper()
    {
        int needDeleteIndex = ControllerSettings.CurControllerIndex;
        _curStatControllerWrapper.ControllerWrapperList.RemoveAt(needDeleteIndex);
        ControllerSettings.CurControllerIndex = _curStatControllerWrapper.ControllerWrapperList.Count > 0? 0 : -1;
    }

    public void DeleteControllerPage(int index)
    {
        ControllerSettings.CurChangeNamePageIndex = -1;
        CurControllerWrapper.RemovePage(index);
    }

    #endregion
    
}