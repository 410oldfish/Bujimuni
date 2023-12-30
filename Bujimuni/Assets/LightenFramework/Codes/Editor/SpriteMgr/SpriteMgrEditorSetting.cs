using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten.Editor
{
    public class SpriteMgrEditorSetting : ScriptableObject
    {
        public const string DEFAULT_PATH = LightenEditorConst.LIGHTEN_EDITOR_CONFIG_DIR + "/SpriteMgrEditorSetting.asset";
        
        [LabelText("输出目录")]
        public string outputFolder;
        
        [LabelText("查询文件夹列表")]
        public List<Object> folders = new List<Object>();
    }
}