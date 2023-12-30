using UnityEngine;

namespace Lighten.Editor
{
    public class TUIEditorSetting : ScriptableObject
    {
        public string uiControllerFolder = "UIControllers";
        
        public string prefixOfAutoBindOnlyGo = "g_";
        public string prefixOfAutoBindAllCom = "w_";
        public string prefixOfIgnoreAutoBind = "i_";
    }
}
