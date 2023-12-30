
namespace Lighten
{
    public class GameEntity : XEntity, IAwake, IDestroy
    {
        public UniTaskCTS UniTaskCTS { get; private set; }
        
        public virtual void Awake()
        {
            this.UniTaskCTS = this.AddComponent<UniTaskCTS>();
        }

        public virtual void Destroy()
        {
            this.UniTaskCTS = null;
        }
    }
}
