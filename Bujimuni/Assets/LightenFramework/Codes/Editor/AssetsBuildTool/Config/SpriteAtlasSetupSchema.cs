#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten.Editor
{
    public class SpriteAtlasSetupSchema : ScriptableObject
    {
        [LabelText("不参与自动图集的目录")]
        public List<Object> ignoreTextureFolders;

        [LabelText("图集分组")] 
        public List<Object> spriteAtlasGroups;
    }
}

#endif