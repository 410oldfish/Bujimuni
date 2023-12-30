using UnityEditor;

namespace Lighten.Editor
{
    public static class TUIAtlasMakerMenu
    {
        [MenuItem(LightenEditorConst.MENU_ROOT + "/图集管理/开始生成", priority = 20)]
        static void GenerateTUISpriteAtlas()
        {
            TUIAtlasMakder.GenerateSpriteAtlasOfUI();
            EditorUtility.DisplayDialog("提示", "完成", "OK");
        }
        [MenuItem(LightenEditorConst.MENU_ROOT + "/图集管理/打开配置", priority = 21)]
        static void OpenUIAtlasMakerSetting()
        {
            var setting = AssetDatabaseExtension.GetOrCreateScriptableObject<UIAtlasMakerSetting>(UIAtlasMakerSetting.DEFAULT_PATH);
            Selection.activeObject = setting;
        }
    }
}

