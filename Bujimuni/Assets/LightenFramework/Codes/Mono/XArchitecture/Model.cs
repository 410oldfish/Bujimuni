
namespace Lighten
{
    public interface IModel : ICanGetArchitecture, ICanSetArchitecture
    {
    }

    public abstract class AbstractModel : AbstractGetSetArchitecture, IModel
    {
        
    }
    
    public interface ICanGetModel : ICanGetArchitecture
    {
    }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : XEntity, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}
