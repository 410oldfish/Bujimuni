// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Threading;
// using Cysharp.Threading.Tasks;
//
// namespace Lighten
// {
//     [Module]
//     public sealed class SpriteManager : Module, ISpriteManager
//     {
//         private Dictionary<string, SpriteData> m_spriteDatas = new Dictionary<string, SpriteData>();
//
//         protected override async UniTask OnInit(Assembly[] assemblies, CancellationToken cancellationToken)
//         {
//             //Utility.Debug.Log(LightenConst.TAG, "111");
//             var config = await LightenEntry.ResourceManager.LoadAssetAsync<SpriteDataConfig>("SpriteDataConfig");
//             //Utility.Debug.Log(LightenConst.TAG, "222");
//             if (config != null)
//             {
//                 InitSpriteData(config);
//                 //Utility.Debug.Log(LightenConst.TAG, "333");
//                 LightenEntry.ResourceManager.ReleaseAsset(config);
//             }
//             Utility.Debug.Log(LightenConst.TAG, $"count: {m_spriteDatas.Count}");
//         }
//
//         public ISpriteData GetSpriteData(string spriteName)
//         {
//             if (m_spriteDatas.ContainsKey(spriteName))
//                 return m_spriteDatas[spriteName];
//             return null;
//         }
//         public void InitSpriteData(SpriteDataConfig config)
//         {
//             m_spriteDatas.Clear();
//             foreach (var data in config.datas)
//             {
//                 foreach (var spriteName in data.spriteNames)
//                 {
//                     if (m_spriteDatas.ContainsKey(spriteName))
//                     {
//                         Utility.Debug.LogError($"重复sprite {spriteName} {data.type} {data.assetName}");
//                         continue;
//                     }
//                     m_spriteDatas.Add(spriteName, new SpriteData()
//                     {
//                         spriteName = spriteName,
//                         spriteType = data.type,
//                         assetName = data.assetName,
//                     });
//                 }
//             }
//         }
//     }
// }
