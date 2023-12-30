using System.Collections.Generic;
using cfg;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

public static partial class ConfigUtils
{
    private static Dictionary<string, byte[]> m_datas = new Dictionary<string, byte[]>();
    private static Dictionary<string, bool> m_loadingConfigs = new Dictionary<string, bool>();

    private static cfg.Tables m_tables;
    public static async UniTask<bool> Initialize()
    {
        m_tables = new Tables();
        await LoadConfigDatas(m_tables.TableNames);
        m_tables.Load(m_datas);
        return true;
    }

    private static async UniTask LoadConfigDatas(List<string> configNames)
    {
        var utss = new List<UniTask>();
        foreach (var configName in configNames)
        {
            Utility.Debug.Log(LightenConst.TAG, $"读取配置数据{configName}");
            if (m_datas.ContainsKey(configName) || m_loadingConfigs.ContainsKey(configName))
            {
                Utility.Debug.LogError($"配置表名重复 {configName}");
                continue;
            }

            var uts = LoadConfigDataAsync(m_datas, configName);
            utss.Add(uts);
        }

        await UniTask.WhenAll(utss);
    }

    private static async UniTask LoadConfigDataAsync(Dictionary<string, byte[]> datas, string configName)
    {
        var resMgr = Game.Architecture.GetManager<IResMgr>();
        m_loadingConfigs.Add(configName, true);
        var configAsset = await resMgr.LoadAssetAsync<TextAsset>(configName);
        datas[configName] = configAsset.bytes;
        m_loadingConfigs.Remove(configName);
        resMgr.ReleaseAsset(configAsset);
    }
}