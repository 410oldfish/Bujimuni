using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
//using HybridCLR;//TODO:暂时不用HybridCLR
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Lighten
{
    [MonoSingletonPath("LightenFramework/AssemblyLoader")]
    public class AssemblyLoader : MonoSingleton<AssemblyLoader>
    {
        private Dictionary<string, Assembly> m_dictHotfixAssemly = new Dictionary<string, Assembly>();
        private List<Assembly> m_listHotfixAssemly = new List<Assembly>();
        
        public List<Assembly> HotfixAssemblies => m_listHotfixAssemly;

        public async UniTask Load()
        {
#if UNITY_EDITOR
            await LoadInEditor();
#else
            await LoadInRuntime();
#endif
        }

#if UNITY_EDITOR
        public async UniTask LoadInEditor()
        {
            var configPath = "Assets/_GameClient/Bundles/Configs/ConfigsOfUnity/HybridCLRAssemblyConfig.asset";
            var config = AssetDatabase.LoadAssetAtPath<HybridCLRAssemblyConfig>(configPath);
            if (config == null)
                return;
            foreach (var hotfixAssemblyName in config.HotfixAssemblyNames)
            {
                var assembly = Assembly.Load(hotfixAssemblyName);
                if (assembly != null)
                {
                    m_dictHotfixAssemly.Add(assembly.FullName, assembly);
                    m_listHotfixAssemly.Add(assembly);
                }
            }
            //TODO:暂时不用HybridCLR
 //            foreach (var hotfixAssemblyName in config.HotfixAssemblyNames)
 //            {
 //                try
 //                {
 //                    byte[] dllDatas =
 // File.ReadAllBytes($"{Application.dataPath}/../HybridCLRData/HotfixAssembly/{hotfixAssemblyName}.dll");
 //                    byte[] pdbDatas =
 // File.ReadAllBytes($"{Application.dataPath}/../HybridCLRData/HotfixAssembly/{hotfixAssemblyName}.pdb");
 //                
 //                    var assembly = Assembly.Load(dllDatas, pdbDatas);
 //                    Debug.Log($"LoadHotfixAssemblies:{hotfixAssemblyName}");
 //                    m_dictHotfixAssemly.Add(hotfixAssemblyName, assembly);
 //                    m_listHotfixAssemly.Add(assembly);
 //                }
 //                catch (System.Exception e)
 //                {
 //                    Debug.LogError(e);
 //                }
 //            }
            await UniTask.CompletedTask;
        }
#endif
        
        private async UniTask LoadInRuntime()
        {
            await UniTask.CompletedTask;
            // //DebugTime.Log("LoadInRuntime");
            // //读取HybridCLRAssemblyConfig
            // var config = await Addressables.LoadAssetAsync<HybridCLRAssemblyConfig>("HybridCLRAssemblyConfig".ToLower());
            // //DebugTime.Log("LoadAssetAsync HybridCLRAssemblyConfig");
            //
            // await LoadAssemblyCodes(config);
            // //AOT补元
            // await LoadMetadataForAOTAssemblies(config);
            // //DebugTime.Log("LoadMetadataForAOTAssemblies");
            // //载入代码
            // try
            // {
            //     await LoadHotfixAssemblies(config);
            //     //DebugTime.Log("LoadHotfixAssemblies");
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError(e.ToString());
            // }
            //
            // Addressables.Release(config);
        }


        public Assembly GetLast()
        {
            if (m_listHotfixAssemly.Count < 1)
                return null;
            return m_listHotfixAssemly[m_listHotfixAssemly.Count - 1];
        }

        private async UniTask LoadMetadataForAOTAssemblies(HybridCLRAssemblyConfig config)
        {
            //TODO:暂时不用HybridCLR
            // HomologousImageMode mode = HomologousImageMode.SuperSet;
            // foreach (var assemblyName in config.AOTMetaAssemblyNames)
            // {
            //     if (!m_dllBytes.ContainsKey(assemblyName))
            //         continue;
            //     LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(m_dllBytes[assemblyName], mode);
            //     Debug.Log($"LoadMetadataForAOTAssembly:{assemblyName}. mode:{mode} ret:{err}");
            //     m_dllBytes.Remove(assemblyName);
            // }
            await UniTask.CompletedTask;
        }

        private async UniTask LoadHotfixAssemblies(HybridCLRAssemblyConfig config)
        {
            foreach (var assemblyName in config.HotfixAssemblyNames)
            {
                Debug.Log($"LoadHotfixAssemblies:{assemblyName}");
                if (!m_dllBytes.ContainsKey(assemblyName))
                    continue;
                byte[] dllBytes = m_dllBytes[assemblyName];
                byte[] pdbBytes = null;
                if (m_pdbBytes.ContainsKey(assemblyName))
                    pdbBytes = m_pdbBytes[assemblyName];
                var assembly = Assembly.Load(dllBytes, pdbBytes);
                Debug.Log($"LoadHotfixAssemblies:{assemblyName} successed");
                m_dictHotfixAssemly.Add(assemblyName, assembly);
                m_listHotfixAssemly.Add(assembly);

                m_dllBytes.Remove(assemblyName);
                if (m_pdbBytes.ContainsKey(assemblyName))
                    m_pdbBytes.Remove(assemblyName);
            }
        }

        private Dictionary<string, byte[]> m_dllBytes = new Dictionary<string, byte[]>();
        private Dictionary<string, byte[]> m_pdbBytes = new Dictionary<string, byte[]>();
        private Dictionary<string, bool> m_states = new Dictionary<string, bool>();

        private async UniTask LoadAssemblyCodes(HybridCLRAssemblyConfig config)
        {
            m_states.Clear();
            m_dllBytes.Clear();
            m_pdbBytes.Clear();
            
            foreach (var assemblyName in config.AOTMetaAssemblyNames)
            {
                if (m_states.ContainsKey(assemblyName))
                    continue;
                m_states.Add(assemblyName, true);
                LoadAssemblyCode(assemblyName).Forget();
            }
            foreach (var assemblyName in config.HotfixAssemblyNames)
            {
                if (m_states.ContainsKey(assemblyName))
                    continue;
                m_states.Add(assemblyName, true);
                LoadAssemblyCode(assemblyName).Forget();
            }

            while (m_states.Count > 0)
            {
                await UniTask.DelayFrame(1);
            }
        }
        private async UniTask LoadAssemblyCode(string assemblyName)
        {
            await UniTask.CompletedTask;
//             TextAsset dllAsset = null;
//             TextAsset pdbAsset = null;
//             byte[] dllDatas = null;
//             byte[] pdbDatas = null;
//
//             var dllName = $"{assemblyName}.dll".ToLower();
//             dllAsset = await Addressables.LoadAssetAsync<TextAsset>(dllName);
//             if (dllAsset != null)
//                 dllDatas = dllAsset.bytes;
// #if UNITY_EDITOR
//             var pdbName = $"{assemblyName}.pdb".ToLower();
//             pdbAsset = await Addressables.LoadAssetAsync<TextAsset>(pdbName);
//             if (pdbAsset != null)
//                 pdbDatas = pdbAsset.bytes;
// #endif
//             m_dllBytes.Add(assemblyName, dllDatas);
//             m_pdbBytes.Add(assemblyName, pdbDatas);
//             Debug.Log($"LoadAssemblyBytes:{assemblyName} successed");
//             if (dllAsset != null)
//                 Addressables.Release(dllAsset);
//             if (pdbAsset != null)
//                 Addressables.Release(pdbAsset);
//             m_states.Remove(assemblyName);
        }
    }
}