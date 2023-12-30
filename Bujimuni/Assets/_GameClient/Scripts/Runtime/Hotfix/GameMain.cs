using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Lighten;

public static class GameMain
{
    public static void Start()
    {
        Utility.Debug.Log(LightenConst.TAG, "GameStart");
        StartGame().Forget();
    }
    public static void Update()
    {
        //Utility.Debug.Log(LightenConst.TAG, "Update");
        //LightenEntry.UpdateModules(Time.deltaTime);
    }
    public static void Finish()
    {
        Utility.Debug.Log(LightenConst.TAG, "GameFinish");
    }

    private static async UniTask<bool> StartGame()
    {
        Utility.Debug.Log(LightenConst.TAG, "StartGame 111");
        //var assembiles = new List<Assembly>();
        //assembiles.Add(Assembly.GetAssembly(typeof(GameMain)));
        //assembiles.Add(Assembly.GetAssembly(typeof(LightenEntry)));
        //await LightenEntry.InitializeModules(assembiles.ToArray());
        Utility.Debug.Log(LightenConst.TAG, "StartGame 222 hehe");
        
        await ConfigUtils.Initialize();
        
        //LightenEntry.UIManager.SeUIConfig(new UIConfig());
        //LightenEntry.GameSaveManager.SetWriter(new GameSaveWriter());
        //LightenEntry.GetModule<GameTaskManager>().AddTaskTrigger<GTaskTrigger_MainGoal>();
        //LightenEntry.GameSceneManager.ChangeScene<MenuScene>();
        var architecture = Game.Architecture;
        architecture.RegisterManager<IGameSceneMgr, GameSceneMgr>();
        architecture.RegisterManager<ISpriteMgr, SpriteMgr>();
        architecture.RegisterManager<IUIMgr, UIMgr>();
        
        architecture.RegisterModel<CircleKiller.CircleKillerModel>();
        
        await Game.Architecture.GetManager<IGameSceneMgr>().ChangeScene<MenuScene>();

        return true;
    }

}
