using System;
using CircleKiller;
using Lighten;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class SamplePlayerController : XEntityController
{
    public float Speed = 10;
    private bool m_isBoost;
    protected override void OnEntityAwake()
    {
        base.OnEntityAwake();
        this.View.SelfPlayerInput.onActionTriggered += OnActionTriggered;
    }
    
    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
        this.View.SelfPlayerInput.onActionTriggered -= OnActionTriggered;
    }
    
    private void OnActionTriggered(InputAction.CallbackContext inputContext)
    {
        //Debug.Log(inputContext.action.name);
        if (inputContext.action.name == "Move")
        {
            if (inputContext.phase == InputActionPhase.Performed)
            {
                var val = inputContext.action.ReadValue<Vector2>();
                //Utility.Debug.Log("GGYY", val.ToString);
                this.View.SelfRigidbody2D.velocity = val * this.Speed * (this.m_isBoost ? 2: 1);
                this.View.Transform.up = val.normalized;
            }

            if (inputContext.phase == InputActionPhase.Canceled)
            {
                this.View.SelfRigidbody2D.velocity = Vector2.zero;
            }
        }

        if (inputContext.action.name == "Fire")
        {
            if (inputContext.phase == InputActionPhase.Started)
            {
                this.m_isBoost = true;
            }

            if (inputContext.phase == InputActionPhase.Canceled)
            {
                this.m_isBoost = false;
            }
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.rigidbody == null)
            return;
        var enemyController = collision.rigidbody.GetComponent<SampleEnemyController>();
        if (enemyController != null)
        {
            Destroy(enemyController.gameObject);
            if (enemyController.Damage > 0)
            {
                var cmd = this.CreateCommand<GameFinishCommand>();
                cmd.IsWin = false;
                this.SendCommand(cmd);
            }
            else
            {
                var cmd = this.CreateCommand<GainScoreCommand>();
                cmd.Score = 1;
                this.SendCommand(cmd);
            }
        }
    }
}