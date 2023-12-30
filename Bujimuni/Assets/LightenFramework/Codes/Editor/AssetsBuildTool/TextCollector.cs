using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lighten;
using Lighten.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;

public static class TextCollector
{
    private const string PREFAB_PATH = LightenEditorConst.LIGHTEN_BUNDLES_PATH + "/Prefabs/UI";             // UI的Prefab路径
    private const string TEXT_COLLECTOR_PATH = "Assets/_GameClient/EditorRes/FontText";
    private const string TEXT_FROM_UI = "TextFromUI.txt";
    private const string TEXT_FROM_DESIGN = "TextFromDesign.txt";
    private const string FINAL_TEXT = "AllText.txt";

    //[MenuItem("Tools/生成字体TXT")]
    public static void Collect()
    {
        // 递归遍历 UIPanel 开头的预制体
        CollectFromUIPrefab(PREFAB_PATH, $"{TEXT_COLLECTOR_PATH}/{TEXT_FROM_UI}");
        GenerateTextFile(TEXT_COLLECTOR_PATH, $"{TEXT_COLLECTOR_PATH}/{FINAL_TEXT}");
        AssetDatabase.Refresh();
    }

    private static void CollectFromUIPrefab(string folderPath, string output)
    {
        // 递归遍历 UIPanel 开头的预制体
        string[] guids = AssetDatabase.FindAssets("UIPanel t:Prefab", new[] { folderPath });
        var sb = new StringBuilder();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            // 在预制体中查找 TextMeshPro 组件，并记录字符
            TMP_Text[] tmps = prefab.GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text tmp in tmps)
            {
                string text = tmp.text;
                if (!string.IsNullOrEmpty(text))
                {
                    sb.Append(text);
                }
            }
            //找ConvenientText组件
            if (prefab.GetComponent<ConvenientText>())
            {
                ConvenientText main = prefab.GetComponent<ConvenientText>();
                foreach (var content in main.datas)
                {
                    string text = content.value;
                    if (!string.IsNullOrEmpty(text))
                    {
                        sb.Append(text);
                    }
                }
            }
            ConvenientText[] convenientTexts = prefab.GetComponentsInChildren<ConvenientText>(true);
            foreach (ConvenientText convenientText in convenientTexts)
            {
                foreach (var content in convenientText.datas)
                {
                    string text = content.value;
                    if (!string.IsNullOrEmpty(text))
                    {
                        sb.Append(text);
                    }
                }
            }
        }
        File.WriteAllText(output, sb.ToString());
    }

    //将目录下所有txt的字合并到一起,生成一个不重复的txt文件
    private static void GenerateTextFile(string folderPath, string output)
    {
        if (File.Exists(output))
        {
            File.Delete(output);
        }
        string[] filePaths = Directory.GetFiles(folderPath, "*.txt");
        HashSet<char> chars = new HashSet<char>();
        foreach (string filePath in filePaths)
        {
            var content = File.ReadAllText(filePath);
            chars.UnionWith(content.Where(c => !char.IsWhiteSpace(c)));
        }
        string strChars = new string(chars.ToArray()).Replace(" ", "").Replace("\n", "").Replace("\r", "");
        File.WriteAllText(output, strChars);
        
        Debug.LogFormat($"Found {strChars.Length} unique characters in all text files. Saved to {output}");
    }
}
