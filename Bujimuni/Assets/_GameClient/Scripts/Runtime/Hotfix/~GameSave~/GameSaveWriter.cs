using System.IO;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

#if WX_MINIGAME && !UNITY_EDITOR

public class GameSaveWriter : IGameSaveWriter
{
    public async UniTask<bool> Save(string saveDataName, string content)
    {
        Utility.Debug.Log("GGYY",$"WX写入存档 {saveDataName}");
        PlayerPrefs.SetString(saveDataName, content);
        PlayerPrefs.Save();
        return true;
    }

    public async UniTask<string> Load(string saveDataName)
    {
        Utility.Debug.Log("GGYY",$"WX读取存档 {saveDataName}");
        return PlayerPrefs.GetString(saveDataName, string.Empty);
    }
}
#else

public class GameSaveWriter : IGameSaveWriter
{
    private string m_foldlerPath = $"{Application.dataPath}/../Documents";
    public async UniTask<bool> Save(string saveDataName, string content)
    {
        var filePath = $"{m_foldlerPath}/{saveDataName}.txt";
        DirectoryExtension.GenerateDirectory(filePath);
        Utility.Debug.Log("GGYY",$"写入存档 {filePath}");
        await File.WriteAllTextAsync(filePath, content);
        return true;
    }

    public async UniTask<string> Load(string saveDataName)
    {
        var filePath = $"{m_foldlerPath}/{saveDataName}.txt";
        DirectoryExtension.GenerateDirectory(filePath);
        Utility.Debug.Log("GGYY",$"读取存档 {filePath}");
        return await File.ReadAllTextAsync(filePath);
    }

    public bool Exist(string saveDataName)
    {
        return false;
    }
}

#endif

