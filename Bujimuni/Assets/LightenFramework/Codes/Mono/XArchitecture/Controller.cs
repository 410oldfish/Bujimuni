
namespace Lighten
{
    public interface IController : ICanGetManager, ICanGetModel, ICanSendCommand, ICanSubscribeEvent, ICanSendQuery
    {
    }
}