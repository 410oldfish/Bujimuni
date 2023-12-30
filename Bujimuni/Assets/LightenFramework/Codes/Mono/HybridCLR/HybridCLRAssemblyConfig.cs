using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten
{
    public class HybridCLRAssemblyConfig : ScriptableObject
    {
        [LabelText("补元-程序集")]
        public List<string> AOTMetaAssemblyNames = new List<string>();
        [LabelText("热更-程序集")]
        public List<string> HotfixAssemblyNames = new List<string>();
    }
}
