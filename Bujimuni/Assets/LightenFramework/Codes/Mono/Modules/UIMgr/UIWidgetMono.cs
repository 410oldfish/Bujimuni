using Sirenix.OdinInspector;
using UnityEngine;

namespace Lighten
{
    public class UIWidgetMono : MonoBehaviour
    {
        [LabelText("控 件 名 称"), DisplayAsString]
        public string WidgetName;
        [LabelText("初 始 状 态")]
        public EUIInitState InitalState = EUIInitState.AutoHide;
        [LabelText("默 认 参 数")]
        public string DefaultParams;
    }
}
