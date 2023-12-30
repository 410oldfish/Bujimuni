
using System.Collections.Generic;

namespace Lighten
{
    public abstract class UIEntity : GameEntity, IController, IUnRegisterableList
    {
        public IArchitecture GetArchitecture()
        {
            return Game.Architecture;
        }

        public List<IUnRegisterable> UnRegisterables { get; private set; } = new List<IUnRegisterable>();
        public ResLoader ResLoader { get; private set; }
        public SpriteLoader SpriteLoader { get; private set; }
        
        public override void Awake()
        {
            base.Awake();
            this.ResLoader = this.AddComponent<ResLoader>();
            this.SpriteLoader = this.AddComponent<SpriteLoader>();
        }

        public override void Destroy()
        {
            base.Destroy();
            this.ResLoader = null;
            this.SpriteLoader = null;
        }
    }
}
