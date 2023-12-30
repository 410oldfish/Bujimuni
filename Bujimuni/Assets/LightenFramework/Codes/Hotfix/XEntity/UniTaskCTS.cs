using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public class UniTaskCTS : XEntity, IAwake, IDestroy
    {
        private CancellationTokenSource m_defaultCts;

        private Dictionary<string, CancellationTokenSource> m_customCtsDict =
            new Dictionary<string, CancellationTokenSource>();

        public class AsyncLock<T> : IDisposable
        {
            private T m_key;
            private Action<T> m_onDispose;

            public static AsyncLock<T> Create(T key, Action<T> onDispose)
            {
                var asyncLock = ObjectPool.Fetch<AsyncLock<T>>();
                asyncLock.Init(key, onDispose);
                return asyncLock;
            }

            public void Init(T key, Action<T> onDispose)
            {
                this.m_key = key;
                this.m_onDispose = onDispose;
            }

            public void Dispose()
            {
                this.m_onDispose?.Invoke(this.m_key);
                ObjectPool.Recycle(this);
            }
        }

        private Dictionary<int, AsyncLock<int>> m_asyncLockDict = new Dictionary<int, AsyncLock<int>>();

        public void Awake()
        {
        }

        public void Destroy()
        {
            this.StopAllTask();
        }

        public void StopAllTask()
        {
            if (m_defaultCts != null)
            {
                m_defaultCts.Cancel();
                m_defaultCts.Dispose();
                m_defaultCts = null;
            }

            if (m_customCtsDict.Count > 0)
            {
                foreach (var cts in m_customCtsDict.Values)
                {
                    cts.Cancel();
                    cts.Dispose();
                }

                m_customCtsDict.Clear();
            }

            if (m_asyncLockDict.Count > 0)
            {
                foreach (var asyncLock in m_asyncLockDict.Values)
                {
                    asyncLock.Dispose();
                }
                m_asyncLockDict.Clear();
            }
        }

        public CancellationToken Default
        {
            get
            {
                if (this.m_defaultCts == null)
                    this.m_defaultCts = new CancellationTokenSource();
                return this.m_defaultCts.Token;
            }
        }

        public CancellationToken LinkedDefault(CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
            {
                return this.Default;
            }

            return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.Default).Token;
        }

        public CancellationTokenSource Create(string name)
        {
            this.Cancel(name);
            var cts = new CancellationTokenSource();
            m_customCtsDict[name] = cts;
            return CancellationTokenSource.CreateLinkedTokenSource(cts.Token, this.Default);
        }

        public void Cancel(string name)
        {
            if (m_customCtsDict.TryGetValue(name, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
                m_customCtsDict.Remove(name);
            }
        }

        public async UniTask<AsyncLock<int>> Lock<T>(T key, CancellationToken cancellationToken = default)
        {
            var hashCode = key.GetHashCode();
            var token = this.LinkedDefault(cancellationToken);
            while (this.m_asyncLockDict.ContainsKey(hashCode))
            {
                await UniTask.DelayFrame(1, cancellationToken: token);
            }

            var asyncLock = AsyncLock<int>.Create(hashCode, this.OnWaitComplete);
            m_asyncLockDict[hashCode] = asyncLock;
            return asyncLock;
        }

        private void OnWaitComplete(int hashCode)
        {
            this.m_asyncLockDict.Remove(hashCode);
        }
    }
}