using UnityEngine;
using UnityEditor;

public abstract class StatControllerPanelBase
{
    protected StatControllerWindow _window;

    protected string _panelName = "UnKnown";

    public string PanelName
    {
        get
        {
            return _panelName;
        }
    }

    public StatControllerPanelBase(StatControllerWindow window)
    {
        _window = window;
    }

    public abstract void OnGUI(Rect panelArea);

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnCurDealUIChanged();

    public abstract void OnUpdate(float deltaTime);
}