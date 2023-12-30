using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputActionMono : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Awake()
    {
        this.playerInput = this.GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput.onActionTriggered += this.OnActionTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log($"Trigger {context.action.name} {context.phase}");
    }
}
