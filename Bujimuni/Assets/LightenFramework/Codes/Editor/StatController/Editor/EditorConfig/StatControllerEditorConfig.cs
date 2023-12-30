using System;
using System.Collections.Generic;
using UnityEngine;


public class StatControllerEditorConfig: ScriptableObject
{
    [SerializeField]
    public List<string> luaFolderPathArray = new List<string>();

    [SerializeField]
    public string luaExporterClass = string.Empty;

    [SerializeField]
    public AnimationClip defaultOpenAni;

    [SerializeField]
    public AnimationClip defaultCloseAni;
}