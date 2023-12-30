using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ControllerDetailScrollView: EditorScrollViewBase
{
    public override void OnGUI(Rect viewArea)
    {
        base.OnGUI(viewArea);
        GUI.BeginGroup(_viewArea, string.Empty);
        DrawDetailItems();
        GUI.EndGroup();
    }

    private void DrawDetailItems()
    {
        float height = StatControllerManager.Instance.ControllerSettings.PanelOffsetY;
        float itemWidth = Mathf.Max(_viewArea.width,
            StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH * StatControllerManager.Instance.CurControllerWrapper.PageNameList.Count);
        if (StatControllerManager.Instance.CurControllerWrapper.ShowTreeList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < StatControllerManager.Instance.CurControllerWrapper.ShowTreeList.Count; i++)
        {
            float itemHeight = StatControllerManager.Instance.CurControllerWrapper.ShowTreeList[i].Height;
            Rect itemArea = new Rect(StatControllerManager.Instance.ControllerSettings.PanelOffsetX,
                height,
                itemWidth,
                itemHeight);
            height += itemHeight;
            StatControllerManager.Instance.CurControllerWrapper.ShowTreeList[i].OnDetailGUI(i, itemArea);
        }

        for (int i = 0; i < StatControllerManager.Instance.CurControllerWrapper.PageNameList.Count; i++)
        {
            Rect cutlineArea = new Rect(
                StatControllerManager.Instance.ControllerSettings.PanelOffsetX + StatControllerEditorUtility.CONTROLLER_PAGE_WIDTH * (i + 1) - 0.5f,
                0,
                1,
                _viewArea.height);
            GUI.DrawTexture(cutlineArea, StatControllerEditorUtility.CutlineTex);
        }
    }
}