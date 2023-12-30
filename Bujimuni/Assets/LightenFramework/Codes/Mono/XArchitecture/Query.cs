
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    public interface IQuery<TResult> : ICanGetArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetManager, ICanSendQuery
    {
        TResult Execute();
    }
    
    public interface IQueryAsync<TResult> : ICanGetArchitecture, ICanSetArchitecture, ICanGetModel, ICanGetManager, ICanSendQuery
    {
        UniTask<TResult> Execute(CancellationToken cancellationToken);
    }
    
    public abstract class AbstractQuery<TResult> : AbstractGetSetArchitecture, IQuery<TResult>
    {
        TResult IQuery<TResult>.Execute()
        {
            return OnExecute();
        }
        protected abstract TResult OnExecute();
    }
    
    public abstract class AbstractQueryAsync<TResult> : AbstractGetSetArchitecture, IQueryAsync<TResult>
    {
        UniTask<TResult> IQueryAsync<TResult>.Execute(CancellationToken cancellationToken)
        {
            return OnExecute(cancellationToken);
        }
        protected abstract UniTask<TResult> OnExecute(CancellationToken cancellationToken);
    }
    
    public interface ICanSendQuery : ICanGetArchitecture
    {
    }

    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult, T>(this ICanSendQuery self) where T : class, IQuery<TResult>, new()
        {
            var query = ObjectPool.Fetch<T>();
            return self.SendQuery(query);
        }
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            query.SetArchitecture(self.GetArchitecture());
            return query.Execute();
        }
        
        public static async UniTask<TResult> SendQueryAsync<TResult, T>(this ICanSendQuery self, CancellationToken cancellationToken) where T : class, IQueryAsync<TResult>, new()
        {
            var query = ObjectPool.Fetch<T>();
            return await self.SendQueryAsync(query, cancellationToken);
        }
        public static async UniTask<TResult> SendQueryAsync<TResult>(this ICanSendQuery self, IQueryAsync<TResult> query, CancellationToken cancellationToken)
        {
            query.SetArchitecture(self.GetArchitecture());
            return await query.Execute(cancellationToken);
        }
    }
}
