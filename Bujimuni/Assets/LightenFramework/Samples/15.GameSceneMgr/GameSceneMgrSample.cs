using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMgrSample : MonoBehaviour, IController
{
    public abstract class SampleScene : GameScene
    {
        protected override async UniTask OnInit(CancellationToken cancellationToken)
        {
            Debug.Log($"OnInit {this.GetType().FullName}");
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnExit(CancellationToken cancellationToken)
        {
            Debug.Log($"OnExit {this.GetType().FullName}");
            await UniTask.CompletedTask;
        }
    }

    public class SceneA : SampleScene
    {
    }

    [GameSceneParent(typeof(SceneA))]
    public class SceneB : SampleScene
    {
    }

    [GameSceneParent(typeof(SceneA))]
    public class SceneC : SampleScene
    {
    }

    [GameSceneParent(typeof(SceneA), typeof(SceneB))]
    public class SceneD : SampleScene
    {
    }

    [GameSceneParent(typeof(SceneA), typeof(SceneB))]
    public class SceneE : SampleScene
    {
    }

    [GameSceneParent(typeof(SceneA), typeof(SceneB), typeof(SceneE))]
    public class SceneF : SampleScene
    {

    }


    private class Architecture : AbstractArchitecture, IAwake
    {
        public void Awake()
        {
            this.RegisterManager<IGameSceneMgr, GameSceneMgr>();
        }
    }

    public Button[] Buttons;

    private List<Type> SceneTypes = new List<Type>()
    {
        typeof(SceneA), typeof(SceneB), typeof(SceneC), typeof(SceneD), typeof(SceneE), typeof(SceneF),
    };

    private void Awake()
    {
        Game.InitArchitecture<Architecture>();
        
        for (int i = 0; i < this.Buttons.Length && i < this.SceneTypes.Count; ++i)
        {
            var sceneType = this.SceneTypes[i];
            this.Buttons[i].onClick.AddListener(() =>
            {
                this.GetManager<IGameSceneMgr>().ChangeScene(sceneType, CancellationToken.None).Forget();
            });
        }
    }

    public IArchitecture GetArchitecture()
    {
        return Game.Architecture;
    }
}