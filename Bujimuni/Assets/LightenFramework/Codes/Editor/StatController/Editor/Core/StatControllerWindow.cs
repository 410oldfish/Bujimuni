using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;


public class StatControllerWindow: EditorWindow
{
    private DateTime _timeStamp;
    private int _refreshRate;

    private void Awake()
    {
        // Debug.Log("On Window Awake");
        base.titleContent = new GUIContent("StatController Editor");
        this.minSize = new Vector2(StatControllerEditorUtility.WINDOW_MIN_WIDTH, StatControllerEditorUtility.WINDOW_MIN_HEIGHT);
        _timeStamp = DateTime.Now;
        _refreshRate = 15;

    }

    private void OnEnable()
    {
        // Debug.Log("On Window Enable");
        StatControllerEditorUtility.Init();
        StatControllerManager.Instance.IsWindowOpen = true;
        StatControllerManager.Instance.LoadLastStatController();
        StatControllerManager.Instance.NeedReInit = true;
        StatControllerManager.Instance.NeedRepaint = true;
        // RefreshDisplayAndWrapperData();
        //StatControllerManager.Instance.CurBindingWrapper.CheckAllLabel();
        EditorApplication.update += (EditorApplication.CallbackFunction)System.Delegate.Combine(EditorApplication.update,
            new EditorApplication.CallbackFunction(StatControllerManager.Instance.OnWindowUpdate));
        // EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.hierarchyWindowItemOnGUI += OnSelectObjectInHierarchy;
    }

    private void OnGUI()
    {
        // 按一定频率强制刷新编辑器界面显示，用来解决因Editor模式下界面刷新较慢而引起的显示异常bug
        double delta = (DateTime.Now - _timeStamp).TotalMilliseconds;
        _timeStamp = DateTime.Now;
        if (delta >= 1f / _refreshRate)
        {
            // Debug.Log(((GameObject)EditorApplication.hierarchyWindowItemOnGUI.Target).name); 

            Repaint();
        }

        Rect toolbarArea = new Rect(0,
            0,
            base.position.width,
            StatControllerEditorUtility.SINGLELINE_HEIGHT);
        DrawToolBar(toolbarArea);

        Rect panelArea = new Rect(0,
            StatControllerEditorUtility.SINGLELINE_SPACING,
            base.position.width,
            base.position.height - StatControllerEditorUtility.SINGLELINE_SPACING);
        DrawPanel(panelArea);
        if (StatControllerManager.Instance.NeedReInit)
        {
            StatControllerManager.Instance.NeedReInit = false;
            StatControllerManager.Instance.Init();
        }

        if (StatControllerManager.Instance.NeedRepaint)
        {
            StatControllerManager.Instance.NeedRepaint = false;
            base.Repaint();
        }
    }

    private void OnSelectObjectInHierarchy(int instanceId, Rect rect)
    {
        GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
        if (Event.current != null && rect.Contains(Event.current.mousePosition)
            && Event.current.button == 0 && Event.current.type < EventType.MouseUp)
        {
            if (selectedGameObject)
            {

                Debug.Log(selectedGameObject.name);
                StatController statController;
#if(UNITY_2019_1_OR_NEWER)
                if (selectedGameObject.TryGetComponent<StatController>(out statController))
#else
                    statController = selectedGameObject.GetComponent<StatController>();
                    if(statController!=null)
#endif
                {
                    // Debug.Log("statController"+statController);
                    ChangeTarget(statController);
                    //StatControllerManager.Instance.WindowSettings.BindingSettings.WorkType = BindingPanelWorkTypeState.Display;
                    Repaint();
                }
                else
                {
                    if (selectedGameObject.transform.parent != null &&
                        TryGetStatControllerFromParent(selectedGameObject.transform.parent.gameObject, out statController))
                    {
                        // Debug.Log("statController"+statController);
                        ChangeTarget(statController);
                        //StatControllerManager.Instance.WindowSettings.BindingSettings.WorkType = BindingPanelWorkTypeState.Display;
                        Repaint();
                    }
                    else
                    {
                        // Debug.Log("No find any StatController in go's relation chain");
                    }
                }
            }
        }
    }

    private bool TryGetStatControllerFromParent(GameObject gameObjectParent, out StatController statController)
    {
        if (gameObjectParent == null)
        {
            statController = null;
            return false;
        }
#if(UNITY_2019_1_OR_NEWER)
        if (gameObjectParent.TryGetComponent<StatController>(out statController))
#else
            statController = gameObjectParent.GetComponent<StatController>();
            if(statController!=null)
#endif
        {
            return true;
        }
        else if (gameObjectParent.transform.parent == null)
        {
            statController = null;
            return false;
        }

        {
            return TryGetStatControllerFromParent(gameObjectParent.transform.parent.gameObject, out statController);
        }
    }

    private void DrawToolBar(Rect toolbarArea)
    {
        float leftX = toolbarArea.x;
        float rightX = toolbarArea.xMax;
        float itemWidth = 0;
        GUI.Box(toolbarArea, string.Empty, EditorStyles.toolbar);

        itemWidth = 200;
        StatControllerManager.Instance.CurStatController =
                EditorGUI.ObjectField(new Rect(leftX, toolbarArea.y, itemWidth, StatControllerEditorUtility.SINGLELINE_HEIGHT),
                    StatControllerManager.Instance.CurStatController, typeof (StatController), true) as StatController;
        leftX += itemWidth;
        if (StatControllerManager.Instance.CurStatController == null)
        {
            return;
        }

        itemWidth = 120;
        GUI.Label(new Rect(leftX, toolbarArea.y, itemWidth, StatControllerEditorUtility.SINGLELINE_HEIGHT), "控制器模式",
            EditorStyles.toolbarButton);

        leftX += itemWidth;

        itemWidth = 80;

        //检测是否是runtime，禁止runtime时进行保存操作
        EditorGUI.BeginDisabledGroup(false);
        if (Application.isPlaying)
        {
            StatControllerManager.Instance.WindowSettings.AutoSave = false;
            EditorGUI.BeginDisabledGroup(true);
        }

        StatControllerManager.Instance.WindowSettings.AutoSave = GUI.Toggle(
            new Rect(rightX - itemWidth, toolbarArea.y, itemWidth, StatControllerEditorUtility.SINGLELINE_HEIGHT),
            StatControllerManager.Instance.WindowSettings.AutoSave, "自动保存");
        EditorGUI.EndDisabledGroup();

        rightX -= itemWidth + 10;
        if (!StatControllerManager.Instance.AutoSave)
        {
            itemWidth = 60;
            if (GUI.Button(new Rect(rightX - itemWidth, toolbarArea.y, itemWidth, StatControllerEditorUtility.SINGLELINE_HEIGHT), "保存",
                    EditorStyles.toolbarButton))
            {
                //检测是否是runtime，禁止runtime时进行保存操作
                if (Application.isPlaying)
                {
                    EditorUtility.DisplayDialog("警告", "Runtime模式不允许保存，请退出Runtime后再保存", "确认");
                    return;
                }

                string failedStr = null;
                if (StatControllerManager.Instance.CurStatControllerWrapper.CheckCanSave(true, out failedStr))
                {
                    StatControllerManager.Instance.CurStatControllerWrapper.Save();
                }
                else
                {
                    EditorUtility.DisplayDialog("保存失败", failedStr, "确认");
                }
            }

            rightX -= itemWidth;
        }

        itemWidth = 80;
        if (GUI.Button(new Rect(rightX - itemWidth, toolbarArea.y, itemWidth, StatControllerEditorUtility.SINGLELINE_HEIGHT), "还原",
                EditorStyles.toolbarButton))
        {
            //RefreshDisplayAndWrapperData();
            StatControllerManager.Instance.NeedReInit = true;
            StatControllerManager.Instance.NeedRepaint = true;
        }

        rightX -= itemWidth;
    }

    private void DrawPanel(Rect panelArea)
    {
        if (StatControllerManager.Instance.CurStatController == null)
        {
            return;
        }

        StatControllerManager.Instance.WindowSettings.Panels[StatControllerManager.Instance.WindowSettings.CurPanelIndex].OnGUI(panelArea);
    }

    private void OnToolbarGenericMenuSelect(object data)
    {
        int index = Mathf.Clamp((int)data, 0, StatControllerManager.Instance.WindowSettings.Panels.Length - 1);
        if (index == StatControllerManager.Instance.WindowSettings.CurPanelIndex)
        {
            return;
        }

        StatControllerManager.Instance.WindowSettings.Panels[StatControllerManager.Instance.WindowSettings.CurPanelIndex].OnExit();
        StatControllerManager.Instance.WindowSettings.CurPanelIndex = index;
        StatControllerManager.Instance.WindowSettings.Panels[StatControllerManager.Instance.WindowSettings.CurPanelIndex].OnEnter();
    }

    private void RefreshDisplayAndWrapperData()
    {
        StatControllerManager.Instance.WindowSettings.Refresh();
        StatControllerManager.Instance.CurStatControllerWrapper.Refresh();
    }

    public void ChangeTarget(StatController tempStatController)
    {
        StatControllerManager.Instance.CurStatController = tempStatController;
        // StatControllerManager.Instance.Init();
    }

    private void OnDisable()
    {
        // EditorApplication.hierarchyWindowItemOnGUI -= OnSelectObjectInHierarchy;
        // EditorApplication.update -= StatControllerManager.Instance.OnWindowUpdate;
        // StatControllerManager.Instance.OnDispose();
        StatControllerManager.Instance.IsWindowOpen = false;
    }

    private void OnDestroy()
    {

        EditorApplication.hierarchyWindowItemOnGUI -= OnSelectObjectInHierarchy;
        EditorApplication.update -= StatControllerManager.Instance.OnWindowUpdate;
        // StatControllerManager.Instance.OnDispose();
        StatControllerManager.Instance.IsWindowOpen = false;
    }

    [MenuItem("Tools/UI/StatController Editor")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<StatControllerWindow>();
    }
}