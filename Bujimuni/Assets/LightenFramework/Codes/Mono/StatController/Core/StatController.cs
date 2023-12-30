using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;



public class StatController: MonoBehaviour
{
    private bool _ready;

    private Controller[] _controllers;

    private Transition[] _transitions;

    private Animation _animation;
    

    [SerializeField]
    private GameObject[] _storedGameObjects;

    [SerializeField]
    private Sprite[] _storedSprites;

    [SerializeField]
    private Material[] _storedMaterials;

    [SerializeField]
    private float[] _storedFloats;

    [SerializeField]
    private int[] _storedInts;

    [SerializeField]
    private string[] _storedStrings;

    [SerializeField]
    private AnimationCurve[] _storedAnimationCurves;

    [SerializeField]
    private ControllerConfig[] _controllerConfigs;

    [SerializeField]
    private TransitionConfig[] _transitionConfigs;
    

    private Tweener _animationEventtweener;
    

    public GameObject[] StoredGameObjects
    {
        get
        {
            return _storedGameObjects;
        }

        set
        {
            _storedGameObjects = value;
        }
    }

    public Sprite[] StoredSprites
    {
        get
        {
            return _storedSprites;
        }

        set
        {
            _storedSprites = value;
        }
    }

    public Material[] StoredMaterials
    {
        get
        {
            return _storedMaterials;
        }
        set
        {
            _storedMaterials = value;
        }
    }


    public float[] StoredFloats
    {
        get
        {
            return _storedFloats;
        }

        set
        {
            _storedFloats = value;
        }
    }

    public int[] StoredInts
    {
        get
        {
            return _storedInts;
        }

        set
        {
            _storedInts = value;
        }
    }

    public string[] StoredStrings
    {
        get
        {
            return _storedStrings;
        }

        set
        {
            _storedStrings = value;
        }
    }

    public AnimationCurve[] StoredAnimationCurves
    {
        get
        {
            return _storedAnimationCurves;
        }

        set
        {
            _storedAnimationCurves = value;
        }
    }

    public ControllerConfig[] ControllerConfigs
    {
        get
        {
            return _controllerConfigs;
        }

        set
        {
            _controllerConfigs = value;
        }
    }

    public TransitionConfig[] TransitionConfigs
    {
        get
        {
            return _transitionConfigs;
        }

        set
        {
            _transitionConfigs = value;
        }
    }

    public bool Ready
    {
        get
        {
            return _ready;
        }
    }

    public Controller[] Controllers
    {
        get
        {
            return _controllers;
        }
    }
    

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (_transitions != null)
        {
            foreach (var transition in _transitions)
            {
                transition.CheckAutoPlay();
            }
        }

    }

    public void Init()
    {
        if (_ready)
        {
            return;
        }

        if (_animation == null)
        {
            _animation = GetComponent<Animation>();
        }

        if (_controllerConfigs != null && _controllerConfigs.Length > 0)
        {
            _controllers = new Controller[_controllerConfigs.Length];
            for (int i = 0; i < _controllerConfigs.Length; i++)
            {
                _controllers[i] = new Controller(this, _controllerConfigs[i]);
            }
        }

        if (_transitionConfigs != null && _transitionConfigs.Length > 0)
        {
            _transitions = new Transition[_transitionConfigs.Length];
            for (int i = 0; i < _transitionConfigs.Length; i++)
            {
                _transitions[i] = new Transition(this, _transitionConfigs[i]);
            }
        }
        

        _ready = true;
    }

    public void ClearStoredDatas()
    {
        _storedGameObjects = null;
        _storedSprites = null;
        _storedFloats = null;
        _storedInts = null;
        _storedStrings = null;
        _storedAnimationCurves = null;

        _controllerConfigs = null;
        _transitionConfigs = null;
    }

    public GameObject GetStoredGameObject(ushort index)
    {
        if (_storedGameObjects == null || index >= _storedGameObjects.Length)
        {
            return null;
        }

        return _storedGameObjects[index];
    }
    
    public void SetController(string name,int index)
    {
        var control = GetController(name);
        if (control!=null)
        {
            control.SelectedIndex = index;
        }
        else
        {
            UnityEngine.Debug.LogWarning("dont has this controller:  "+name);
        }
    }
        
    public int GetControllerSelectedIndex(string name)
    {
        var control = GetController(name);
        if (control != null)
        {
            return control.SelectedIndex;
        }
        else
        {
            UnityEngine.Debug.LogWarning("dont has this controller:  " + name);
        }
        return -1;
    }

    public string GetControllerSelectedPageName(string name)
    {
        var control = GetController(name);
        if (control != null && control.PageNum > 0)
        {
            foreach (var controllerConfig in _controllerConfigs)
            {
                if (controllerConfig.name == name)
                {
                    return controllerConfig.pageNames[control.SelectedIndex];
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("dont has this controller:  " + name);
        }
        return null;
    }

    public Controller GetController(int index)
    {
        if (_controllers == null || index < 0 || index >= _controllers.Length)
        {
            return null;
        }

        return _controllers[index];
    }

    public Controller GetController(string name)
    {
        if (_controllers == null || _controllers.Length == 0)
        {
            return null;
        }

        foreach (var controller in _controllers)
        {
            if (controller.Name == name)
            {
                return controller;
            }
        }

        return null;
    }

    public void EditorChangeControllerSelectedIndex(string controllerName, int index)
    {
        if (_controllerConfigs != null && _controllerConfigs.Length > 0)
        {
            for (int i = 0; i < _controllerConfigs.Length; i++)
            {
                if (_controllerConfigs[i].name == controllerName)
                {
                    Controller controller = new Controller(this, _controllerConfigs[i]);
                    controller.EditorApply(index);
                    break;
                }
            }
        }
    }

    public ControllerConfig EditorGetControllerConfig(string name)
    {
        if (_controllerConfigs == null || _controllerConfigs.Length == 0)
        {
            return null;
        }

        foreach (var controllerConfig in _controllerConfigs)
        {
            if (controllerConfig.name == name)
            {
                return controllerConfig;
            }
        }

        return null;
    }

    public Transition GetTransiton(string name)
    {
        if (_transitions == null || _transitions.Length == 0)
        {
            UnityEngine.Debug.LogWarning("dont has this GetTransiton: " + name);
            return null;
        }

        foreach (var transiton in _transitions)
        {
            if (transiton.Name == name)
            {
                return transiton;
            }
        }

        return null;
    }

    public void PlayAnimation(string animName, UnityEngine.Events.UnityAction onComplete = null)
    {
        if (_animation == null)
        {
            onComplete?.Invoke();
            UnityEngine.Debug.LogWarning("dont has animation:" + animName);
            return;
        }

        if (_animation[animName] != null)
        {
            bool bPlay = _animation.Play(animName);
            if (bPlay)
            {
                if (onComplete != null)
                {
                    _animationEventtweener?.Kill();
                    float delay = _animation.GetClip(animName).length;
                    _animationEventtweener = TweenManager.Instance.CreateTweener().SetDelay(delay)
                            .OnComplete(() => { onComplete(); });
                }
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning(string.Format("<color=#ffff00>播放动画[{0}]失败: 未能找到对应的Animation</color>", animName));
            onComplete?.Invoke();
        }
    }

    public void StopAnimation(string animName)
    {
        if (_animation == null)
        {
            UnityEngine.Debug.LogWarning("dont has animation:" + animName);
            return;
        }

        _animation.Stop(animName);
        _animationEventtweener?.Kill();
    }

    /// <summary>
    /// Use to play all transitions which are contained by StatController
    /// </summary>
    public void PlayAllTransitions()
    {
        if (_transitionConfigs == null || _transitionConfigs.Length <= 0)
        {
            return;
        }

        if (_transitions == null || _transitions.Length <= 0 || _transitions.Length != _transitionConfigs.Length)
        {
            _transitions = null;
            _transitions = new Transition[_transitionConfigs.Length];
            for (int i = 0; i < _transitionConfigs.Length; i++)
            {
                _transitions[i] = new Transition(this, _transitionConfigs[i]);
            }
        }

        for (int i = 0; i < _transitionConfigs.Length; i++)
        {
            PlayTransiton(_transitionConfigs[i].name);
        }
    }

    public void PlayTransiton(string transitionName, int times = 1, float delay = 0, UnityEngine.Events.UnityAction onComplete = null,
    bool reverse = false)
    {
        Transition transition = GetTransiton(transitionName);
        if (transition == null)
        {
            UnityEngine.Debug.LogWarning("dont has this Transition: " + transitionName);
        }
        else
        {
            transition.Play(times, delay, onComplete, reverse);
        }
    }

    // 预留
    public void PauseTransiton(string transitionName, bool pauseState)
    {
        Transition transition = GetTransiton(transitionName);
        if (transition == null)
        {
            UnityEngine.Debug.LogWarning("dont has this Transition: " + transitionName);
            return;
        }

        transition.SetPaused(pauseState);
    }

    public void StopTransiton(string transitionName)
    {
        Transition transition = GetTransiton(transitionName);
        if (transition == null)
        {
            UnityEngine.Debug.LogWarning("dont has this Transition: " + transitionName);
            return;
        }

        transition.Stop();
    }
}