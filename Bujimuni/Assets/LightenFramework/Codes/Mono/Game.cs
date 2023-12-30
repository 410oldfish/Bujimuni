
namespace Lighten
{
    public static class Game
    {
        public static XEntity Root => XEntityMgr.Instance.Root;
        public static IArchitecture Architecture { get; private set; }

        public static void InitArchitecture<T>() where T : XEntity, IArchitecture
        {
            Architecture = Root.AddComponent<T>();
            Architecture.Init();
        }
    }
}
