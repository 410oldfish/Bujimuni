using System.Collections;
using System.Collections.Generic;
using Lighten;
using UnityEngine;

public class SmallSquare : HotfixMono
{
    public override void Update()
    {
        base.Update();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        Utility.Debug.Log($"SmallSquare OnCollisionEnter2D {collision.gameObject.name}");
    }
    
}
