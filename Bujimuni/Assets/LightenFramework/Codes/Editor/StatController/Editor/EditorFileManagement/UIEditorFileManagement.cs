using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;


public class UIEditorFileManagement: UnityEditor.AssetModificationProcessor
{


    public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
    {
        // UnityEngine.Debug.Log("资源即将被移动 form:" + oldPath + " to:" + newPath);
        GameObject assetGo = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(oldPath);
        if (assetGo != null && assetGo.GetComponent<StatController>() != null)
        {
            // UnityEngine.Debug.Log("检测到是StatController");
            List<string> useItemAssetPaths = Find(oldPath);
            if (useItemAssetPaths.Count > 0)
            {
                foreach (var assetPath in useItemAssetPaths)
                {
                    bool hasChanged = false;
                    UnityEngine.Debug.Log(assetPath);
                    GameObject uesedGo = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(assetPath);
                    if (uesedGo != null && uesedGo.GetComponent<StatController>() != null)
                    {
                        StatController statController = uesedGo.GetComponent<StatController>();

                        for (int i = 0; i < statController.StoredStrings.Length; i++)
                        {
                            /*string luaOldPath = oldPath.Replace(".prefab", "");
                            luaOldPath = luaOldPath.Replace(StatControllerBindingPanelSettings.PrefabPath,
                                StatControllerBindingPanelSettings.LuaFilePath);
                            string luaNewPath = newPath.Replace(".prefab", "");
                            luaNewPath = luaNewPath.Replace(StatControllerBindingPanelSettings.PrefabPath,
                                StatControllerBindingPanelSettings.LuaFilePath);*/
                            /*if (statController.StoredStrings[i] == luaOldPath)
                            {
                                // UnityEngine.Debug.Log("我找到了： "+ luaOldPath);
                                statController.StoredStrings[i] = luaNewPath;
                                // UnityEngine.Debug.Log("进行了替换： " + luaOldPath + " ==> " + luaNewPath);
                                hasChanged = true;
                            }*/
                        }
                    }

                    if (hasChanged)
                    {
                        PrefabUtility.SavePrefabAsset(uesedGo, out bool success);
                    }

                    AssetDatabase.Refresh();
                }
            }

            return AssetMoveResult.DidNotMove;
        }
        else
        {
            return AssetMoveResult.DidNotMove;
        }

    }

    /*
    public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
    {
        // UnityEngine.Debug.Log("资源即将被删除 : " + assetPath);
        return AssetDeleteResult.DidNotDelete;
    }
    */

    private const string MenuItemName = "Assets/Find References In Project %#&f";
    private const string MetaExtension = ".meta";


    public static List<string> Find(string targetAssetPath)
    {
        List<string> usefilePathList = new List<string>();
        bool isMacOS = Application.platform == RuntimePlatform.OSXEditor;
        int totalWaitMilliseconds = isMacOS? 2 * 1000 : 300 * 1000;
        int cpuCount = Environment.ProcessorCount;
        string appDataPath = Application.dataPath;

        // var selectedObject = Selection.activeObject;
        string selectedAssetPath = targetAssetPath;
        string selectedAssetGUID = AssetDatabase.AssetPathToGUID(selectedAssetPath);
        string selectedAssetMetaPath = selectedAssetPath + MetaExtension;

        var references = new List<string>();
        var output = new System.Text.StringBuilder();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var psi = new ProcessStartInfo();
        psi.WindowStyle = ProcessWindowStyle.Minimized;

        if (isMacOS)
        {
            psi.FileName = "/usr/bin/mdfind";
            psi.Arguments = string.Format("-onlyin {0} {1}", appDataPath, selectedAssetGUID);
        }
        else
        {
            psi.FileName = Path.Combine(Environment.CurrentDirectory, @"Assets\ui.expansion\Editor\Tools\rg.exe");
            psi.Arguments = string.Format("--case-sensitive --follow --files-with-matches --no-text --fixed-strings " +
                // "--ignore-file Assets/Editor/FindReferencesInProject2/ignore.txt " +
                "--threads {0} --regexp {1} -- {2}",
                cpuCount, selectedAssetGUID, appDataPath);
        }

        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        var process = new Process();
        process.StartInfo = psi;

        process.OutputDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            string relativePath = e.Data.Replace(appDataPath, "Assets").Replace("\\", "/");

            // skip the meta file of whatever we have selected
            if (relativePath == selectedAssetMetaPath)
                return;

            references.Add(relativePath);
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Data))
                return;

            output.AppendLine("Error: " + e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        while (!process.HasExited)
        {
            if (stopwatch.ElapsedMilliseconds < totalWaitMilliseconds)
            {
                float progress = (float)((double)stopwatch.ElapsedMilliseconds / totalWaitMilliseconds);
                string info = string.Format("Finding {0}/{1}s {2:P2}", stopwatch.ElapsedMilliseconds / 1000,
                    totalWaitMilliseconds / 1000, progress);
                bool canceled = EditorUtility.DisplayCancelableProgressBar("Find References in Project", info, progress);

                if (canceled)
                {
                    process.Kill();
                    break;
                }

                Thread.Sleep(100);
            }
            else
            {
                process.Kill();
                break;
            }
        }

        foreach (string file in references)
        {
            string guid = AssetDatabase.AssetPathToGUID(file);
            output.AppendLine(string.Format("{0} {1}", guid, file));

            string assetPath = file;
            if (file.EndsWith(MetaExtension))
            {
                assetPath = file.Substring(0, file.Length - MetaExtension.Length);
            }

            // UnityEngine.Debug.Log(string.Format("{0}\n{1}", file, guid), AssetDatabase.LoadMainAssetAtPath(assetPath));
            usefilePathList.Add(assetPath);
        }

        EditorUtility.ClearProgressBar();
        stopwatch.Stop();
        return usefilePathList;
        /*
        string content = string.Format(
            "{0} {1} found for object: \"{2}\" path: \"{3}\" guid: \"{4}\" total time: {5}s\n\n{6}",
            references.Count, references.Count > 2 ? "references" : "reference", selectedObject.name, selectedAssetPath,
            selectedAssetGUID, stopwatch.ElapsedMilliseconds / 1000d, output);
        UnityEngine.Debug.LogWarning(content, selectedObject);
        */

    }

    private static bool FindValidate()
    {
        var obj = Selection.activeObject;
        if (obj != null && AssetDatabase.Contains(obj))
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return !AssetDatabase.IsValidFolder(path);
        }

        return false;
    }
}