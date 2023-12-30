using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
//using UnityEngine.AddressableAssets;
using Lighten;

public class LaunchScene : MonoBehaviour
{
    async UniTask Start()
    {
        //第一步更新Addressable
        //await Addressables.InitializeAsync();
        
        // await AssemblyLoader.Instance.Load();
        //
        // var entryAssembly = AssemblyLoader.Instance.GetLast();
        // if (entryAssembly != null)
        // {
        //     AssemblyRunner.Instance.Run(entryAssembly);
        // }
    }
}