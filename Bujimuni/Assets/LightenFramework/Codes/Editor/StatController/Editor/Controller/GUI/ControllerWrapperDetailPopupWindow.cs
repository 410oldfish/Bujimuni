using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


    public class ControllerWrapperEditAction
    {
        public enum EditActionStateType
        {
            Remove,
            Add,
        }

        public ControllerWrapperEditAction(EditActionStateType state, int pageIndex)
        {
            m_editActionState = state;
            m_editPageIndex = pageIndex;
        }

        public EditActionStateType m_editActionState;

        public int m_editPageIndex;
    }

public class ControllerWrapperDetailPopupWindow: EditorWindow
{
    private string _wrapperName = "";

    private List<string> _pageNameList = new List<string>();
    private List<string> _pageTipsList = new List<string>(); //标签备注

    private Vector2 _scrollViewVec2 = new Vector2();

    private List<ControllerWrapperEditAction> _editActionList;

    private static void ShowWindow()
    {
        EditorWindow.GetWindow<ControllerWrapperDetailPopupWindow>();
    }

    public Vector2 GetWindowSize()
    {
        return new Vector2(500, 190);
    }

    public void OnGUI()
    {
        GUILayout.Label("控制器名称：", EditorStyles.boldLabel);
        _wrapperName = GUILayout.TextField(_wrapperName);
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("状态（只能输入字母和数字）：", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.Label("备注：", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", GUILayout.Width(20)))
        {
            _pageNameList.Add("");
            //标签备注
            _pageTipsList.Add("");
            if (StatControllerManager.Instance.ControllerSettings.InEditState)
            {
                _editActionList.Add(new ControllerWrapperEditAction(ControllerWrapperEditAction.EditActionStateType.Add, 0));
            }
        }

        GUILayout.EndHorizontal();
        _scrollViewVec2 = GUILayout.BeginScrollView(_scrollViewVec2, GUILayout.Height(86));
        for (int i = 0; i < _pageNameList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(i + ":", GUILayout.Width(20));
            _pageNameList[i] = EditorGUILayout.TextField(_pageNameList[i]);
            //标签备注
            _pageTipsList[i] = EditorGUILayout.TextField(_pageTipsList[i]);
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                _pageNameList.RemoveAt(i);
                _pageTipsList.RemoveAt(i);
                if (StatControllerManager.Instance.ControllerSettings.InEditState)
                {
                    _editActionList.Add(new ControllerWrapperEditAction(ControllerWrapperEditAction.EditActionStateType.Remove, i));
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("保存", GUILayout.Width(60)))
        {
            OnSaveBtnClick();
        }

        GUILayout.EndHorizontal();
    }

    private void OnSaveBtnClick()
    {
        if (StatControllerManager.Instance.ControllerSettings.InCreateNewWrapperState)
        {
            if (string.IsNullOrEmpty(_wrapperName))
            {
                EditorUtility.DisplayDialog("保存失败", "控制器名称不能为空。", "确认");
                return;
            }

            if (!IsNumAndEnCh(_wrapperName))
            {
                EditorUtility.DisplayDialog("保存失败", "控制器名称只能包含字母与数字。", "确认");
                return;
            }

            foreach (StatControllerControllerWrapper controller in StatControllerManager.Instance.CurStatControllerWrapper.ControllerWrapperList)
            {
                if (controller.Name == _wrapperName)
                {
                    EditorUtility.DisplayDialog("保存失败", "已存在相同名称的控制器。", "确认");
                    return;
                }
            }

            if (_pageNameList.Count < 2)
            {
                EditorUtility.DisplayDialog("保存失败", "控制器状态数量必须大于2。", "确认");
                return;
            }

            Debug.Log(_pageTipsList.Count);
            StatControllerManager.Instance.CreateNewControllerWrapper(_wrapperName, _pageNameList, _pageTipsList);
            // this.editorWindow.Close();
            this.Close();
        }
        else if (StatControllerManager.Instance.ControllerSettings.InEditState)
        {
            if (string.IsNullOrEmpty(_wrapperName))
            {
                EditorUtility.DisplayDialog("保存失败", "控制器名称不能为空。", "确认");
                return;
            }

            if (!IsNumAndEnCh(_wrapperName))
            {
                EditorUtility.DisplayDialog("保存失败", "控制器名称只能包含字母与数字。", "确认");
                return;
            }

            foreach (StatControllerControllerWrapper controller in StatControllerManager.Instance.CurStatControllerWrapper.ControllerWrapperList)
            {
                if ((controller != StatControllerManager.Instance.CurControllerWrapper) && controller.Name == _wrapperName)
                {
                    EditorUtility.DisplayDialog("保存失败", "已存在相同名称的控制器。", "确认");
                    return;
                }
            }

            if (_pageNameList.Count < 2)
            {
                EditorUtility.DisplayDialog("保存失败", "控制器页面数量必须大于2。", "确认");
                return;
            }

            StatControllerManager.Instance.CurControllerWrapper.ApplyEditData(_editActionList, _wrapperName, _pageNameList, _pageTipsList);
            // this.editorWindow.Close();
            this.Close();
        }
    }

    public bool IsNumAndEnCh(string input)
    {
        string pattern = @"^[A-Za-z0-9]+$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(input);
    }

    // public override void OnOpen()
    public void Awake()
    {
        if (StatControllerManager.Instance.ControllerSettings.InCreateNewWrapperState)
        {
            _wrapperName = "";
            _pageNameList = new List<string>() { "", "" };
            _pageTipsList = new List<string>() { "", "" };
        }

        if (StatControllerManager.Instance.ControllerSettings.InEditState)
        {
            _wrapperName = StatControllerManager.Instance.CurControllerWrapper.Name;
            _pageNameList = new List<string>();
            _pageTipsList = new List<string>();
            _editActionList = new List<ControllerWrapperEditAction>();
            for (int i = 0; i < StatControllerManager.Instance.CurControllerWrapper.PageNameList.Count; i++)
            {
                _pageNameList.Add(StatControllerManager.Instance.CurControllerWrapper.PageNameList[i]);
                _pageTipsList.Add(StatControllerManager.Instance.CurControllerWrapper.PageTipsList[i]);
            }
        }
    }

    public void OnDestroy()
    {
        if (StatControllerManager.Instance.ControllerSettings.InCreateNewWrapperState)
        {
            StatControllerManager.Instance.ControllerSettings.InCreateNewWrapperState = false;
        }

        if (StatControllerManager.Instance.ControllerSettings.InEditState)
        {
            StatControllerManager.Instance.ControllerSettings.InEditState = false;
        }

    }
}