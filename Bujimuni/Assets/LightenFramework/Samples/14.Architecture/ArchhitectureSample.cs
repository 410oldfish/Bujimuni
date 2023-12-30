using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lighten;
using UnityEngine;

namespace Lighten.Sample
{
    public class ArchhitectureSample : XEntityMonoBehaviour<XPureEntity>, IController
    {
        #region 数据
        public class SimpleModel : AbstractModel, IAwake
        {
            public RxProperty<int> Count = new RxProperty<int>();
        
            public void Awake()
            {
                this.Count.Value = 10;
            }
        }
        #endregion
        
        #region 查询
        
        public class QueryCountName: AbstractQueryAsync<string>
        {
            protected override async UniTask<string> OnExecute(CancellationToken cancellationToken)
            {
                Debug.Log("查询中...");
                await UniTask.Delay(1000, cancellationToken: cancellationToken);
                return $"查询结果:{this.GetModel<SimpleModel>().Count}";
            }
        }
        
        #endregion
        
        #region 命令
        public class IncreaseCommand : AbstractCommand
        {
            protected override void OnExecute()
            {
                var model = this.GetModel<SimpleModel>();
                model.Count.Value += 1;
                if (model.Count >= 100)
                {
                    this.PublishEvent<CountArrived100Event>();
                }
            }
        }
        public class DecreaseCommand : AbstractCommand
        {
            protected override void OnExecute()
            {
                this.GetModel<SimpleModel>().Count.Value -= 1;
            }
        }
        
        public class DoubleValueCommand : AbstractCommand<int>
        {
            protected override int OnExecute()
            {
                return this.GetModel<SimpleModel>().Count.Value * 2;
            }
        }
        
        public class IncreaseValueAsyncCommand : AbstractAsyncCommand
        {
            public int Count;
            
            protected override async UniTask OnExecute(CancellationToken token)
            {
                var model = this.GetModel<SimpleModel>();
                for (int i = 0; i < Count; ++i)
                {
                    model.Count.Value += 1;
                    await this.PublishEventAsync(new ShowDebugTextEvent() { Text = $"数量+1 {model.Count}" }, token);
                    await UniTask.Delay(500, cancellationToken: token);
                }
                var result = await this.SendQueryAsync<string, QueryCountName>(token);
                Debug.Log(result);
            }
        }
        #endregion
        
        #region 事件
        
        public struct CountArrived100Event
        {
        }
        
        public struct ShowDebugTextEvent
        {
            public string Text;
        }
        
        #endregion
        
        private class Architecture : AbstractArchitecture, IAwake
        {
            public void Awake()
            {
                //注册Manager
                this.RegisterManager<IResMgr, ResMgrByYooAsset>();
                
                //注册Model
                this.RegisterModel<SimpleModel>();
            }
        }

        protected override void OnEntityAwake()
        {
            base.OnEntityAwake();
            Game.InitArchitecture<Architecture>();
            this.GetManager<IResMgr>().Predownload();
            this.GetManager<IResMgr>().LoadAssetAsync<Mesh>("AABB").Forget();
            this.GetModel<SimpleModel>().Count.Value = 99;
        }

        public IArchitecture GetArchitecture()
        {
            return Game.Architecture;
        }
    }
}