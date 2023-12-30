using System;
using Lighten;
using UnityEngine;

public partial class SampleEnemyController : XEntityController
{
    public int Damage = 0;
    public float SafeTime = 1;
    
    protected override void OnEntityAwake()
    {
        base.OnEntityAwake();
        this.SetDamage(0);
    }

    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    private void Update()
    {
        if (this.SafeTime > 0)
        {
            this.SafeTime -= Time.deltaTime;
            if (this.SafeTime <= 0)
            {
                this.SetDamage(1);
            }
        }
    }
    
    public void SetDamage(int damage)
    {
        this.Damage = damage;
        if (this.Damage > 0)
        {
            this.View.RenderSpriteRenderer.color = Color.red;
        }
        else
        {
            this.View.RenderSpriteRenderer.color = Color.green;
        }
    }
}