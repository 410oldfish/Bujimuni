using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameObjectManagerSample : MonoBehaviour
{
    public string assetName;
    public Transform root;
    
    private List<GameObject> m_gameObjects = new List<GameObject>();
    async void Start()
    {
        await LightenEntry.InitializeModules();
        Spawn().Forget();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        await Recycle();
    }

    async UniTask<int> Spawn()
    {
        for (int i = 0; i < 100; ++i)
        {
            var go = await LightenEntry.GameObjectManager.InstantiateAsync(assetName);
            go.transform.SetParent(root,false);
            go.transform.localPosition = Random.insideUnitSphere * 50f;
            m_gameObjects.Add(go);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        return 0;
    }

    async UniTask<int> Recycle()
    {
        while (m_gameObjects.Count > 0)
        {
            LightenEntry.GameObjectManager.Recycle(m_gameObjects[0]);
            m_gameObjects.RemoveAt(0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
        return 0;
    }
}
