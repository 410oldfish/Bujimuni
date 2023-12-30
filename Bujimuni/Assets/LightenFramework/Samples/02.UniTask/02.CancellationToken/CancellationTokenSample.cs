using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CancellationTokenSample : MonoBehaviour
{
    public string assetName;
    // Start is called before the first frame update
    async void Start()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var go = await LoadAssetAsync<GameObject>(assetName, cancellationTokenSource.Token);
        
        if (go != null)
        {
            Debug.Log($"加载完成 {go.name}");
        }
        else
        {
            Debug.Log($"加载取消");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async UniTask<T> LoadAssetAsync<T>(string assetName, CancellationToken cancellationToken) where T : Object
    {
        await UniTask.CompletedTask;
        return null;
        // assetName = assetName.ToLower();
        // var result = await Addressables.LoadAssetAsync<T>(assetName).WithCancellation(cancellationToken).SuppressCancellationThrow();
        // if (result.IsCanceled)
        // {
        //     return null;
        // }
        //     
        // return result.Result;
    }
}
