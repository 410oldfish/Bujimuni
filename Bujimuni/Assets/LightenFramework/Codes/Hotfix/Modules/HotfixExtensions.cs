using System;

namespace Lighten
{
    public static class HotfixExtensions
    {
        //
        public static GameScene CurrentScene(this GameEntity entity)
        {
            var gameSceneMgr = Game.Architecture.GetManager<IGameSceneMgr>(false);
            if (gameSceneMgr != null)
            {
                return gameSceneMgr.GetCurrentScene<GameScene>();
            }

            return entity.GlobalScene();
        }

        //
        public static UIComponent CurrentUI(this GameEntity entity)
        {
            return entity.CurrentScene().UIComponent;
        }
        
        public static UIComponent CurrentUI(this GameScene gameScene)
        {
            return gameScene.UIComponent;
        }

        //
        public static GameScene GlobalScene(this GameEntity entity)
        {
            var globalScene = Game.Root.GetComponent<GlobalScene>();
            if (globalScene == null)
            {
                globalScene = Game.Root.AddComponent<GlobalScene>();
            }

            return globalScene;
        }

        //
        public static UIComponent GlobalUI(this GameEntity entity)
        {
            return entity.GlobalScene().UIComponent;
        }
    }
}