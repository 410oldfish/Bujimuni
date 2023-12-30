using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Lighten
{
    public interface IGameSceneMgr
    {
        T GetCurrentScene<T>() where T : XEntity, IGameScene;
        UniTask ChangeScene(Type sceneType, CancellationToken cancellationToken = default);
        UniTask ChangeScene<T>(CancellationToken cancellationToken = default) where T : XEntity, IGameScene;
    }

    public class GameSceneMgr : AbstractManager, IGameSceneMgr, IAwake, IDestroy
    {
        private UniTaskCTS m_uniTaskCts;
        private Stack<IGameScene> m_currSceneStack = new Stack<IGameScene>();
        private List<IGameScene> m_removedScenes = new List<IGameScene>();
        private bool m_isChangingScene = false;

        public void Awake()
        {
            this.m_uniTaskCts = this.AddComponent<UniTaskCTS>();
        }

        public void Destroy()
        {
            this.m_currSceneStack.Clear();
        }
        
        public T GetCurrentScene<T>() where T : XEntity, IGameScene
        {
            return this.m_currSceneStack.Count > 0 ? this.m_currSceneStack.Peek() as T : null;
        }

        public async UniTask ChangeScene(Type sceneType, CancellationToken cancellationToken = default)
        {
            var token = this.m_uniTaskCts.LinkedDefault(cancellationToken);
            while (this.m_isChangingScene)
            {
                await UniTask.DelayFrame(1, cancellationToken: token);
            }

            m_isChangingScene = true;
            await ChangeSceneInternal(sceneType, token);
            m_isChangingScene = false;
        }

        public async UniTask ChangeScene<T>(CancellationToken cancellationToken = default) where T : XEntity, IGameScene
        {
            await ChangeScene(typeof(T), cancellationToken);
        }

        private async UniTask ChangeSceneInternal(Type sceneType, CancellationToken cancellationToken)
        {
            Debug.Log($"切换至场景 {sceneType.FullName}");
            m_removedScenes.Clear();
            XEntity nextSceneParent = this;

            var nextScenePath = this.GetScenePath(sceneType);
            int nextScenePathStartIndex = 0;
            //弹出当前场景,直到与目标场景契合
            while (this.m_currSceneStack.Count > 0)
            {
                var currentScene = this.m_currSceneStack.Peek();
                var currentSceneType = currentScene.GetType();
                //如果已经是当前场景,那么直接跳出
                if (currentSceneType == sceneType)
                {
                    nextSceneParent = null;
                    break;
                }

                var lastIndex = this.LastIndex(nextScenePath, currentSceneType);
                if (lastIndex != -1 && currentScene is XEntity entity)
                {
                    nextSceneParent = entity;
                    nextScenePathStartIndex = lastIndex + 1;
                    break;
                }

                m_currSceneStack.Pop();
                await currentScene.Exit(cancellationToken);
                m_removedScenes.Add(currentScene);
            }

            //依次创建新场景的父结构场景
            if (nextScenePath != null && nextSceneParent != null)
            {
                for (int i = nextScenePathStartIndex; i < nextScenePath.Length; ++i)
                {
                    var parentSceneType = nextScenePath[i];
                    var parentScene = (IGameScene)nextSceneParent.AddChild(parentSceneType);
                    await parentScene.Init(cancellationToken);
                    this.m_currSceneStack.Push(parentScene);
                    nextSceneParent = (XEntity)parentScene;
                }
            }
            //创建新场景
            if (nextSceneParent != null)
            {
                var newScene = (IGameScene)nextSceneParent.AddChild(sceneType);
                if (!string.IsNullOrEmpty(newScene.SceneAssetName))
                {
                    if (nextSceneParent != this)
                    {
                        throw new Exception($"根节点场景才能切换UnityScene!! {sceneType.Name}不是根节点场景");
                    }
                    await this.GetManager<IResMgr>().LoadSceneAsync(newScene.SceneAssetName, cancellationToken: cancellationToken);
                }

                await newScene.Init(cancellationToken);
                this.m_currSceneStack.Push(newScene);
            }

            //移除移除旧场景
            if (m_removedScenes.Count > 0)
            {
                foreach (var scene in m_removedScenes)
                {
                    ((IDisposable)scene).Dispose();
                }

                m_removedScenes.Clear();
            }

            await UniTask.CompletedTask;
        }

        private Type[] GetScenePath(Type sceneType)
        {
            var attributes = Utility.Assembly.GetAttributes<GameSceneParentAttribute>(sceneType);
            if (attributes.Length == 0)
            {
                return null;
            }

            return attributes[0].ParentTypes;
        }

        private int LastIndex(Type[] parentTypes, Type sceneType)
        {
            if (parentTypes == null || parentTypes.Length < 1)
            {
                return -1;
            }

            for (int i = parentTypes.Length - 1; i >= 0; --i)
            {
                if (parentTypes[i] == sceneType)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}