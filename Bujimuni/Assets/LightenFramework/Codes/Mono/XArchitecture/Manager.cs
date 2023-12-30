

namespace Lighten
{
    public interface IManager : ICanGetArchitecture, ICanSetArchitecture, ICanGetManager, ICanGetModel, ICanPublishEvent, ICanSubscribeEvent
    {
        
    }
    public abstract class AbstractManager : AbstractGetSetArchitecture, IManager
    {
    }
    
    public interface ICanGetManager : ICanGetArchitecture
    {
    }

    public static class CanGetManagerExtension
    {
        public static T GetManager<T>(this ICanGetManager self) where T : class
        {
            return self.GetArchitecture().GetManager<T>();
        }
    }
}
