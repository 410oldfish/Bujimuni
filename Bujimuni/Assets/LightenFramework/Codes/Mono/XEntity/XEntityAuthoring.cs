using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lighten
{
    /// <summary>
    /// 这个类用于挂载在GameObject上,在EntityMgr中会为它创建对应的Entity,并且为他创建完整的树结构
    /// </summary>
    public class XEntityAuthoring : MonoBehaviour
    {
        //TODO:可以做一个检索工具
        public string ClassName;
    }
}
