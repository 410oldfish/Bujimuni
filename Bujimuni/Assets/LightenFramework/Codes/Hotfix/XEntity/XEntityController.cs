
namespace Lighten
{
    public abstract class XEntityController : XEntityMonoBehaviour<GameEntity>, IController
    {
        protected override void OnEntityAwake()
        {
            base.OnEntityAwake();
            OnInitComponents();
        }
        
        protected abstract void OnInitComponents();

        public IArchitecture GetArchitecture()
        {
            return Game.Architecture;
        }
    }
}