using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GearTypeState: int
{
    // StatController
    Controller = 0,
    Transition = 1,

    // GameObject
    Active = 2,

    // RectTransform
    Position = 3,
    SizeDelta = 4,
    Rotation = 5,
    Scale = 6,

    // Text
    TextStr = 7,
    TextColor = 8,


    // Image
    ImageSprite = 9,
    ImageColor = 10,

    RawImageColor = 11,
    ImageMaterial = 12,

    //new Feature
    OverallAlpha = 13,
    PercentPosition = 16,
}

public enum LineTypeState : int
    {
        Event = 0,

        Active = 1,

        Position = 2,
        Rotation = 3,
        Scale = 4,
        SizeDelta = 5,

        TextColor = 6,

        ImageSprite = 7,
        ImageColor = 8,
    }

public enum LinkerValueState: int
{
    Vector2 = 0,
    Vector3 = 1,
    Quaternion = 2,
    Boolean = 3,
    Int32 = 4,
    String = 5,
    Single = 6,
    Color = 7,
    Sprite = 8,
    Char = 9,
    Rect = 10,
    SystemObject = 11,

    UnityEvent = 100,
    UnityEventBoolean = 101,
    UnityEventSingle = 102,
    UnityEventInt32 = 103,
    UnityEventString = 104,
    UnityEventVector2 = 105,
}

public enum EaseType: int
{
    Linear = 0,
    SineIn = 1,
    SineOut = 2,
    SineInOut = 3,
    QuadIn = 4,
    QuadOut = 5,
    QuadInOut = 6,
    CubicIn = 7,
    CubicOut = 8,
    CubicInOut = 9,
    QuartIn = 10,
    QuartOut = 11,
    QuartInOut = 12,
    QuintIn = 13,
    QuintOut = 14,
    QuintInOut = 15,
    ExpoIn = 16,
    ExpoOut = 17,
    ExpoInOut = 18,
    CircIn = 19,
    CircOut = 20,
    CircInOut = 21,
    ElasticIn = 22,
    ElasticOut = 23,
    ElasticInOut = 24,
    BackIn = 25,
    BackOut = 26,
    BackInOut = 27,
    BounceIn = 28,
    BounceOut = 29,
    BounceInOut = 30,
    Custom = 31,
    None = 100,
}

public class StatControllerUtility
{
    public const ushort TrueValue = 1;

    public const ushort FalseValue = 0;

    private static Dictionary<string, System.Type> m_RegisterBinderDic = null;
    private static RectTransform m_RootCanvas;

    private static List<string> _registerBinderKeyList = null;

    public static List<string> RegisterBinderKeyList
    {
        get
        {
            if (_registerBinderKeyList == null)
            {
                _registerBinderKeyList = new List<string>();
            }

            return _registerBinderKeyList;
        }
    }

    private static RectTransform FindUIRootCanvas()
    {
        var uiroot = UnityEngine.GameObject.Find("TUIRoot");
        if (uiroot == null)
        {
            return null;
        }
        var rootcanvas = uiroot.transform.Find("UICanvas");
        return rootcanvas.GetComponent<RectTransform>();
    }

    public static void GetCanvasSize(out float width, out float height)
    {
        if (m_RootCanvas == null)
        {
            m_RootCanvas = FindUIRootCanvas();
        }

        if (m_RootCanvas != null)
        {
            var rect = m_RootCanvas.rect;
            width = rect.width;
            height = rect.height;
        }
        else
        {
            Debug.LogWarning("UI Root Cancas Not find!");
            width = 1920;
            height = 1080;
        }
    }

    public static GearBase GetGear(Controller parent, GearConfig config)
    {
        switch (config.gearType)
        {
            case GearTypeState.Controller:
                return new ControllerGear(parent, config);
            case GearTypeState.Transition:
                return null;
            case GearTypeState.Active:
                return new ActiveGear(parent, config);
            case GearTypeState.OverallAlpha:
                return new OverallAlphaGear(parent, config);
            case GearTypeState.Position:
                return new PositionGear(parent, config);
            case GearTypeState.PercentPosition:
                return new PercentPositionGear(parent, config);
            case GearTypeState.SizeDelta:
                return new SizeDeltaGear(parent, config);
            case GearTypeState.Rotation:
                return new RotationGear(parent, config);
            case GearTypeState.Scale:
                return new ScaleGear(parent, config);
            case GearTypeState.TextStr:
                return new TextStrGear(parent, config);
            case GearTypeState.TextColor:
                return new TextColorGear(parent, config);
            case GearTypeState.ImageSprite:
                return new ImageSpriteGear(parent, config);
            case GearTypeState.ImageColor:
                return new ImageColorGear(parent, config);
            case GearTypeState.ImageMaterial:
                return new ImageMaterialGear(parent, config);
            case GearTypeState.RawImageColor:
                return new RawImageColorGear(parent, config);
            default:
                return null;
        }
    }

    public static KeyFrameBase GetKeyFrame(Transition parent, KeyFrameConfig config)
    {
        switch (config.lineType)
        {
            case LineTypeState.Position:
                return new PositionKeyFrame(parent, config);
            case LineTypeState.Active:
                return new ActiveKeyFrame(parent, config);
            case LineTypeState.Event:
                return new EventKeyFrame(parent, config);
            case LineTypeState.ImageColor:
                return new ImageColorKeyFrame(parent, config);
            case LineTypeState.ImageSprite:
                return new ImageSpriteKeyFrame(parent, config);
            case LineTypeState.Rotation:
                return new RotationKeyFrame(parent, config);
            case LineTypeState.Scale:
                return new ScaleKeyFrame(parent, config);
            case LineTypeState.SizeDelta:
                return new SizeDeltaKeyFrame(parent, config);
            case LineTypeState.TextColor:
                return new TextColorKeyFrame(parent, config);
            default:
                return null;
        }
    }
}