using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StatControllerControllerPanel: StatControllerPanelBase
{
    private ControllerPageScrollView _pageView;

    private ControllerHeaderScrollView _headerView;

    private ControllerDetailScrollView _detailView;

    private Rect _previousPanelArea = default (Rect);

    private Rect _borderControlArea = default (Rect);

    private Rect _toolbarArea = default (Rect);

    private Rect _secondToolbarArea = default (Rect);

    private Rect _headerArea = default (Rect);

    private Rect _pageNameArea = default (Rect);

    private Rect _detailArea = default (Rect);

    private Rect _horizontalSliderBarArea = default (Rect);

    private Rect _verticalSliderBarArea = default (Rect);

    private string _tempControllerWrapperName;

    private string _newPageName;

    private float _headerAreaWidth = 198;

    private float _headerAreaMinWidth = 198;

    private float _headerAreaMaxWidth = 298;

    private float _borderResizeOffsetX = 0f;

    public StatControllerControllerPanel(StatControllerWindow window): base(window)
    {
        _panelName = "Controller";
        _pageView = new ControllerPageScrollView();
        _headerView = new ControllerHeaderScrollView();
        _detailView = new ControllerDetailScrollView();
    }

    public override void OnCurDealUIChanged()
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
        StatControllerManager.Instance.ControllerSettings.ExitAllState();
    }

    public override void OnGUI(Rect panelArea)
    {
        UpdatePanelLayout(panelArea);
        DrawToolbarArea();
        if (StatControllerManager.Instance.CurControllerWrapper != null)
        {
            UpdateWrapperData();
            DrawSecondToolbar();
            DrawBorderLine();
            _pageView.OnGUI(_pageNameArea);
            _headerView.OnGUI(_headerArea);
            _detailView.OnGUI(_detailArea);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (StatControllerManager.Instance.ControllerSettings.InRecordingState)
        {
            int[] selecteds = Selection.instanceIDs;
            for (int i = 0; i < selecteds.Length; i++)
            {
                StatControllerManager.Instance.CurControllerWrapper.MonitorGameObjectChange(selecteds[i]);
            }
        }
    }

    private void UpdateWrapperData()
    {
        float pageNameListRealWidth = StatControllerManager.Instance.CurControllerWrapper.PageNameList.Count *
                StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH;
        float horizontalSliderBarMax = 0;
        float horizontalSliderBarItemWidth = 0;
        if (pageNameListRealWidth < _pageNameArea.width)
        {
            StatControllerManager.Instance.ControllerSettings.PanelOffsetX = 0;
            horizontalSliderBarMax = _pageNameArea.width;
            horizontalSliderBarItemWidth = _pageNameArea.width;
        }
        else
        {
            StatControllerManager.Instance.ControllerSettings.PanelOffsetX = Mathf.Max(_pageNameArea.width - pageNameListRealWidth,
                StatControllerManager.Instance.ControllerSettings.PanelOffsetX);
            horizontalSliderBarMax = pageNameListRealWidth;
            horizontalSliderBarItemWidth = _pageNameArea.width;
        }

        StatControllerManager.Instance.ControllerSettings.PanelOffsetX = -GUI.HorizontalScrollbar(_horizontalSliderBarArea,
            -StatControllerManager.Instance.ControllerSettings.PanelOffsetX,
            horizontalSliderBarItemWidth, 0, horizontalSliderBarMax);

        // StatControllerEditorData.Instance.ControllerWrapper.RebuildDisplayTree();
        float showTreeHeight = StatControllerManager.Instance.CurControllerWrapper.ShowTreeHeight + 30;
        float verticalSliderBarMax = 0;
        float verticalSliderBarItemHeight = 0;
        if (showTreeHeight < _detailArea.height)
        {
            StatControllerManager.Instance.ControllerSettings.PanelOffsetY = 0;
            verticalSliderBarMax = _detailArea.height;
            verticalSliderBarItemHeight = _detailArea.height;
        }
        else
        {
            StatControllerManager.Instance.ControllerSettings.PanelOffsetY = Mathf.Max(_detailArea.height - showTreeHeight,
                StatControllerManager.Instance.ControllerSettings.PanelOffsetY);
            verticalSliderBarMax = showTreeHeight;
            verticalSliderBarItemHeight = _detailArea.height;
        }

        Event e = Event.current;
        var delta = 0f;
        if (e.isScrollWheel)
        {
            delta = e.delta.y * StatControllerManager.Instance.WindowSettings.MouseScrollWheelSteps;
        }

        StatControllerManager.Instance.ControllerSettings.PanelOffsetY = -GUI.VerticalScrollbar(_verticalSliderBarArea,
            -StatControllerManager.Instance.ControllerSettings.PanelOffsetY + delta,
            verticalSliderBarItemHeight, 0, verticalSliderBarMax);
    }

    private void DrawSecondToolbar()
    {
        Rect backgroundArea = new Rect(_secondToolbarArea);
        backgroundArea.width += StatControllerEditorUtility.BORDER_CONTROL_WIDTH;
        GUI.Box(backgroundArea, string.Empty, "TE Toolbar");

        float leftX = _secondToolbarArea.x;
        float rightX = _secondToolbarArea.xMax;
        //float tempItemWidth = 30;
        /*
        tempItemWidth = 20;
        if (GUI.Button(new Rect(rightX - tempItemWidth, _secondToolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), StatControllerEditorUtility.AddIconTex, EditorStyles.toolbarButton))
        {
            StatControllerManager.Instance.CurControllerWrapper.AddNewPage(_newPageName);
            _newPageName = string.Empty;
        }
        rightX -= tempItemWidth;

        tempItemWidth = 160;
        _newPageName = GUI.TextField(new Rect(leftX, _secondToolbarArea.y, rightX, StatControllerEditorUtility.SINGLELINE_SPACING), _newPageName);
        leftX += tempItemWidth;
        */
    }

    private void UpdatePanelLayout(Rect panelArea)
    {
        _toolbarArea = new Rect(panelArea.x,
            panelArea.y,
            panelArea.width,
            StatControllerEditorUtility.SINGLELINE_SPACING);

        _borderControlArea = new Rect(panelArea.x + _headerAreaWidth,
            panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING,
            StatControllerEditorUtility.BORDER_CONTROL_WIDTH,
            panelArea.height - StatControllerEditorUtility.SINGLELINE_SPACING);

        if (UpdateHeaderAreaWidth() || panelArea != _previousPanelArea)
        {
            float rightAreaPosX = panelArea.x + _headerAreaWidth + StatControllerEditorUtility.BORDER_CONTROL_WIDTH;
            float rightAreaWidth = panelArea.xMax - rightAreaPosX - StatControllerEditorUtility.SLIDER_BAR_WIDTH;

            _secondToolbarArea = new Rect(panelArea.x,
                panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING,
                _headerAreaWidth,
                StatControllerEditorUtility.SINGLELINE_SPACING);

            _headerArea = new Rect(panelArea.x,
                panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING * 2,
                _headerAreaWidth,
                panelArea.height - StatControllerEditorUtility.SINGLELINE_SPACING * 2 - StatControllerEditorUtility.SINGLELINE_SPACING);

            _pageNameArea = new Rect(rightAreaPosX,
                panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING,
                rightAreaWidth,
                StatControllerEditorUtility.SINGLELINE_SPACING);

            _detailArea = new Rect(rightAreaPosX,
                panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING * 2,
                rightAreaWidth,
                panelArea.height - StatControllerEditorUtility.SINGLELINE_SPACING * 2 - StatControllerEditorUtility.SINGLELINE_SPACING);

            _horizontalSliderBarArea = new Rect(rightAreaPosX,
                panelArea.yMax - StatControllerEditorUtility.SLIDER_BAR_WIDTH,
                rightAreaWidth,
                StatControllerEditorUtility.SLIDER_BAR_WIDTH);

            _verticalSliderBarArea = new Rect(panelArea.xMax - StatControllerEditorUtility.SLIDER_BAR_WIDTH,
                panelArea.y + StatControllerEditorUtility.SINGLELINE_SPACING,
                StatControllerEditorUtility.SLIDER_BAR_WIDTH,
                panelArea.height - StatControllerEditorUtility.SINGLELINE_SPACING - StatControllerEditorUtility.SLIDER_BAR_WIDTH);
        }

        _previousPanelArea = panelArea;
    }

    private bool UpdateHeaderAreaWidth()
    {
        bool widthChanged = false;
        EditorGUIUtility.AddCursorRect(_borderControlArea, MouseCursor.ResizeHorizontal);
        int controlID = GUIUtility.GetControlID("Controller Panel Header Width Change".GetHashCode(), FocusType.Passive, _borderControlArea);
        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                if (_borderControlArea.Contains(Event.current.mousePosition) && Event.current.button == 0)
                {
                    _borderResizeOffsetX = Event.current.mousePosition.x - _headerAreaWidth;
                    GUIUtility.hotControl = controlID;
                    Event.current.Use();
                }

                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                }

                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    float tempWidth = Event.current.mousePosition.x - _borderResizeOffsetX;
                    _headerAreaWidth = Mathf.Clamp(Event.current.mousePosition.x, _headerAreaMinWidth, _headerAreaMaxWidth);
                    StatControllerManager.Instance.NeedRepaint = true;
                    widthChanged = true;
                }

                break;
        }

        return widthChanged;
    }

    private void DrawToolbarArea()
    {
        GUI.Box(_toolbarArea, string.Empty, EditorStyles.toolbar);
        float leftX = _toolbarArea.x;
        float rightX = _toolbarArea.xMax;
        float tempItemWidth = 0;

        /*
        if (StatControllerManager.Instance.ControllerSettings.InChangeNameState)
        {
            tempItemWidth = 160;
            _tempControllerWrapperName = GUI.TextField(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), _tempControllerWrapperName);
            leftX += tempItemWidth;

            tempItemWidth = 20;
            if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), "+", EditorStyles.toolbarButton))
            {
                StatControllerManager.Instance.CurControllerWrapper.ChangeCurControllerName(_tempControllerWrapperName);
                StatControllerManager.Instance.ControllerSettings.InChangeNameState = false;
            }
            leftX += tempItemWidth;

            tempItemWidth = 20;
            if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), "-", EditorStyles.toolbarButton))
            {

                StatControllerManager.Instance.ControllerSettings.InChangeNameState = false;
            }
            leftX += tempItemWidth;
        }
        else
        {
            tempItemWidth = 200;
            if (StatControllerManager.Instance.ControllerSettings.CurControllerIndex == -1)
            {
                if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), "Create New Controller", EditorStyles.toolbarButton))
                {
                    StatControllerManager.Instance.CreateNewControllerWrapper();
                }
            }
            else
            {
                if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), StatControllerManager.Instance.CurControllerWrapper.Name, EditorStyles.toolbarDropDown))
                {
                    OnSelectControllerBtnClick();
                }
            }
            leftX += tempItemWidth;
        }
        */

        tempItemWidth = 200;
        if (StatControllerManager.Instance.ControllerSettings.CurControllerIndex == -1)
        {
            if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), "Create New Controller",
                    EditorStyles.toolbarButton))
            {

                // PopupWindow.Show(buttonRect, new ControllerChangeNamePopupWindow());
                // StatControllerManager.Instance.CreateNewControllerWrapper();
                CreateNewWrapper();
            }
        }
        else
        {
            if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING),
                    StatControllerManager.Instance.CurControllerWrapper.Name, EditorStyles.toolbarDropDown))
            {
                OnSelectControllerBtnClick();
            }
        }

        leftX += tempItemWidth;

        if (StatControllerManager.Instance.CurControllerWrapper == null)
        {
            return;
        }

        Color tempColor = GUI.color;

        if (StatControllerManager.Instance.ControllerSettings.InRecordingState)
        {
            GUI.color = new Color(0.5f, 1, 1, 1f);
        }

        tempItemWidth = 120;
        StatControllerManager.Instance.ControllerSettings.InRecordingState = GUI.Toggle(
            new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING),
            StatControllerManager.Instance.ControllerSettings.InRecordingState, "自动记录", EditorStyles.toolbarButton);
        leftX += tempItemWidth; // + 10;
        GUI.color = tempColor;

        if (StatControllerManager.Instance.ControllerSettings.AutoApply)
        {
            GUI.color = new Color(0.5f, 1, 1, 1f);
        }

        tempItemWidth = 120;
        StatControllerManager.Instance.ControllerSettings.AutoApply = GUI.Toggle(
            new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING),
            StatControllerManager.Instance.ControllerSettings.AutoApply, "自动预览", EditorStyles.toolbarButton);
        leftX += tempItemWidth + 10;
        GUI.color = tempColor;
        if (!StatControllerManager.Instance.ControllerSettings.AutoApply && !StatControllerManager.Instance.ControllerSettings.InRecordingState)
        {
            tempItemWidth = 80;
            if (GUI.Button(new Rect(leftX, _toolbarArea.y, tempItemWidth, StatControllerEditorUtility.SINGLELINE_SPACING), "预览",
                    EditorStyles.toolbarButton))
            {

                StatControllerManager.Instance.CurControllerWrapper.Apply();
            }

            leftX += tempItemWidth;
        }
    }

    private void OnSelectControllerBtnClick()
    {
        GenericMenu controllerListMenu = new GenericMenu();
        if (StatControllerManager.Instance.CurStatControllerWrapper.ControllerWrapperList.Count > 0)
        {
            for (int i = 0; i < StatControllerManager.Instance.CurStatControllerWrapper.ControllerWrapperList.Count; i++)
            {
                var controllerWrapper = StatControllerManager.Instance.CurStatControllerWrapper.ControllerWrapperList[i];
                controllerListMenu.AddItem(new GUIContent(controllerWrapper.Name),
                    i == StatControllerManager.Instance.ControllerSettings.CurControllerIndex, OnChangeSelectWrapperBtnClick, i);
            }

            controllerListMenu.AddSeparator(string.Empty);
        }

        controllerListMenu.AddItem(new GUIContent("Create"), false, CreateNewWrapper);
        controllerListMenu.AddItem(new GUIContent("Edit"), false, ChangeWrapperInChangeNameState);
        controllerListMenu.AddItem(new GUIContent("Delete"), false, StatControllerManager.Instance.DeleteCurControllerWrapper);
        controllerListMenu.ShowAsContext();
    }

    private void CreateNewWrapper()
    {
        StatControllerManager.Instance.ControllerSettings.InCreateNewWrapperState = true;
        Rect buttonRect = new Rect(0, _toolbarArea.y, 200, StatControllerEditorUtility.SINGLELINE_SPACING);
        var pos = EditorWindow.GetWindow<StatControllerWindow>().position.position;
        pos.y += 42;
        EditorWindow.GetWindow<ControllerWrapperDetailPopupWindow>().position = new Rect(pos, new Vector2(500, 190));

    }

    private void ChangeWrapperInChangeNameState()
    {
        StatControllerManager.Instance.ControllerSettings.InEditState = true;
        Rect buttonRect = new Rect(0, _toolbarArea.y, 200, StatControllerEditorUtility.SINGLELINE_SPACING);
        var pos = EditorWindow.GetWindow<StatControllerWindow>().position.position;
        pos.y += 42;
        EditorWindow.GetWindow<ControllerWrapperDetailPopupWindow>().position = new Rect(pos, new Vector2(500, 190));
    }

    private void OnChangeSelectWrapperBtnClick(object indexObj)
    {
        int wrapperIndex = (int)indexObj;
        StatControllerManager.Instance.ControllerSettings.ChangeCurControllerWrapper(wrapperIndex);
    }

    private void DrawBorderLine()
    {
        Rect textureArea = new Rect(_borderControlArea);
        textureArea.y += StatControllerEditorUtility.SINGLELINE_SPACING;
        textureArea.height -= StatControllerEditorUtility.SINGLELINE_SPACING;
        GUI.DrawTexture(textureArea, StatControllerEditorUtility.CutlineTex);
    }
}