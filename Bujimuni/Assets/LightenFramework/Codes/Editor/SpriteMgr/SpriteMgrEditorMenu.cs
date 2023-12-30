using UnityEditor;

namespace Lighten.Editor
{
    public static class SpriteMgrEditorMenu
    {
        
        [MenuItem(LightenEditorConst.MENU_ROOT + "/精灵管理/开始生成", priority = 30)]
        static void OpenWindow()
        {
            var successed = SpriteMgrEditorHelper.GenerateSpriteDataConfig();
            if (successed)
            {
                EditorUtility.DisplayDialog("提示", "精灵管理配置生成成功~~~", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("提示", "精灵管理配置生成失败!!!", "确定");
            }
        }
        
        [MenuItem(LightenEditorConst.MENU_ROOT + "/精灵管理/打开配置", priority = 31)]
        static void OpenSpriteMgrEditorSetting()
        {
            var spriteMgrEditorSetting = AssetDatabaseExtension.GetOrCreateScriptableObject<SpriteMgrEditorSetting>(SpriteMgrEditorSetting.DEFAULT_PATH);
            Selection.activeObject = spriteMgrEditorSetting;
        }
    }
}
