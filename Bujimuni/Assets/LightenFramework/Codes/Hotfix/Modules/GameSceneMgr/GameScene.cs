using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GameSceneParentAttribute : Attribute
    {
        public Type[] ParentTypes { get; }

        public GameSceneParentAttribute(params Type[] parentTypes)
        {
            this.ParentTypes = parentTypes;
        }
    }

    public interface IGameScene : IController
    {
        string SceneAssetName { get; set; }
        UniTask Init(CancellationToken cancellationToken = default);
        UniTask Exit(CancellationToken cancellationToken = default);
    }
    
    public abstract class GameScene : XEntity, IGameScene, IAwake, IDestroy
    {
        public virtual string SceneAssetName { get; set; }
        
        public UIComponent UIComponent { get; private set; }
        
        protected bool IsActived { get; private set; }
        protected bool IsInitialized { get; private set; }
        
        public void Awake()
        {
            this.UIComponent = this.AddComponent<UIComponent>();
        }
        
        public virtual void Destroy()
        {
            if (this.IsActived)
            {
                this.Exit().Forget();
            }
        }
        
        public IArchitecture GetArchitecture()
        {
            return Game.Architecture;
        }

        public async UniTask Init(CancellationToken cancellationToken = default)
        {
            this.IsActived = true;
            await this.OnInit(cancellationToken);
            this.IsInitialized = true;
        }

        public async UniTask Exit(CancellationToken cancellationToken = default)
        {
            this.IsActived = false;
            await this.OnExit(cancellationToken);
        }
        
        protected abstract UniTask OnInit(CancellationToken cancellationToken = default);
        protected abstract UniTask OnExit(CancellationToken cancellationToken = default);
    }

    public class GlobalScene : GameScene
    {
        protected override async UniTask OnInit(CancellationToken cancellationToken = default)
        {
            await UniTask.CompletedTask;
        }
        
        protected override async UniTask OnExit(CancellationToken cancellationToken = default)
        {
            await UniTask.CompletedTask;
        }
    }
}
