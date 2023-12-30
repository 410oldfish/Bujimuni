using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAOTTypes : MonoBehaviour
{
    List<Type> GetTypes()
    {
        return new List<Type>
        {
            #region Unity物理组件
            
            typeof(Rigidbody2D),
            typeof(BoxCollider2D),
            typeof(CircleCollider2D),
            typeof(CapsuleCollider2D),
            typeof(CompositeCollider2D),
            typeof(EdgeCollider2D),
            typeof(PolygonCollider2D),
            typeof(CustomCollider2D),
            
            #endregion
        };
    }
}
