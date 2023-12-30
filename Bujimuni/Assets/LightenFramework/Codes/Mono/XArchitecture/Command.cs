using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Lighten
{
    //普通命令
    public interface ICommand : ICanGetArchitecture, ICanSetArchitecture, ICanGetManager, ICanGetModel, ICanPublishEvent, ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }
    
    //带返回值的命令
    public interface ICommand<TResult> : ICanGetArchitecture, ICanSetArchitecture, ICanGetManager, ICanGetModel, ICanPublishEvent, ICanSendCommand, ICanSendQuery
    {
        TResult Execute();
    }
    
    //异步执行的命令
    public interface IAsyncCommand : ICanGetArchitecture, ICanSetArchitecture, ICanGetManager, ICanGetModel, ICanPublishEvent, ICanSendCommand, ICanSendQuery
    {
        UniTask Execute(CancellationToken cancellationToken);
    }
    
    //异步执行带返回值的命令
    public interface IAsyncCommand<TResult> : ICanGetArchitecture, ICanSetArchitecture, ICanGetManager, ICanGetModel, ICanPublishEvent, ICanSendCommand, ICanSendQuery
    {
        UniTask<TResult> Execute(CancellationToken cancellationToken);
    }

    public abstract class AbstractCommand : AbstractGetSetArchitecture, ICommand
    {
        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }
    
    public abstract class AbstractCommand<TResult> : AbstractGetSetArchitecture, ICommand<TResult>
    {
        TResult ICommand<TResult>.Execute()
        {
            return OnExecute();
        }

        protected abstract TResult OnExecute();
    }
    
    public abstract class AbstractAsyncCommand : AbstractGetSetArchitecture, IAsyncCommand
    {
        UniTask IAsyncCommand.Execute(CancellationToken cancellationToken)
        {
            return OnExecute(cancellationToken);
        }

        protected abstract UniTask OnExecute(CancellationToken cancellationToken);
    }
    
    public abstract class AbstractAsyncCommand<TResult> : AbstractGetSetArchitecture, IAsyncCommand<TResult>
    {
        UniTask<TResult> IAsyncCommand<TResult>.Execute(CancellationToken cancellationToken)
        {
            return OnExecute(cancellationToken);
        }

        protected abstract UniTask<TResult> OnExecute(CancellationToken cancellationToken);
    }

    public interface ICanSendCommand : ICanGetArchitecture
    {
    }

    public static class CanSendCommandExtension
    {
        public static T CreateCommand<T>(this ICanSendCommand self) where T : class, new()
        {
            return ObjectPool.Fetch<T>();
        }
        
        public static void SendCommand<T>(this ICanSendCommand self) where T : class, ICommand, new()
        {
            var command = self.CreateCommand<T>();
            self.SendCommand(command);
        }

        public static void SendCommand(this ICanSendCommand self, ICommand command)
        {
            command.SetArchitecture(self.GetArchitecture());
            command.Execute();
            ObjectPool.Recycle(command);
        }
        
        public static TResult SendCommand<TResult, T>(this ICanSendCommand self) where T : class, ICommand<TResult>, new()
        {
            var command = self.CreateCommand<T>();
            return self.SendCommand(command);
        }
        
        public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command)
        {
            command.SetArchitecture(self.GetArchitecture());
            var result = command.Execute();
            ObjectPool.Recycle(command);
            return result;
        }
        
        public static async UniTask SendCommandAsync<T>(this ICanSendCommand self, CancellationToken cancellationToken) where T : class, IAsyncCommand, new()
        {
            var command = self.CreateCommand<T>();
            await self.SendCommandAsync(command, cancellationToken);
        }
        
        public static async UniTask SendCommandAsync(this ICanSendCommand self, IAsyncCommand command, CancellationToken cancellationToken)
        {
            command.SetArchitecture(self.GetArchitecture());
            await command.Execute(cancellationToken);
            ObjectPool.Recycle(command);
        }
        
        public static async UniTask<TResult> SendCommandAsync<TResult, T>(this ICanSendCommand self, CancellationToken cancellationToken) where T : class, IAsyncCommand<TResult>, new()
        {
            var command = self.CreateCommand<T>();
            return await self.SendCommandAsync(command, cancellationToken);
        }
        
        public static async UniTask<TResult> SendCommandAsync<TResult>(this ICanSendCommand self, IAsyncCommand<TResult> command, CancellationToken cancellationToken)
        {
            command.SetArchitecture(self.GetArchitecture());
            var result = await command.Execute(cancellationToken);
            ObjectPool.Recycle(command);
            return result;
        }

    }
}