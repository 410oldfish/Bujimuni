using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten.Editor
{
    public class UIAtlasMakerSetting : ScriptableObject
    {
        public const string DEFAULT_PATH = LightenEditorConst.LIGHTEN_EDITOR_CONFIG_DIR + "/UIAtlasMakerSetting.asset";
        public const string ATLAS_PREFAB_PATH = LightenEditorConst.LIGHTEN_BUNDLES_PATH + "/UI";
        public const string ATLAS_OUTPUT_PATH = LightenEditorConst.LIGHTEN_BUNDLES_PATH + "/Atlas";
        
        [LabelText("忽略目录")]
        public List<Object> ignoreTextureFolders;

        [LabelText("图集分组")] 
        public List<Object> spriteAtlasGroups;
    }
}