
namespace Lighten
{
    public interface ISingleton
    {
        //单例初始化
        void OnSingletonInit();
        //销毁单例
        void OnSingletonDestroy();
    }
}
