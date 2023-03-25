using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 move { get; private set; }
    public Vector2 look { get; private set; }
    public bool sprint { get; private set; }
    public bool jumpHeld { get; private set; }

    // Delegates & Events
    public delegate void JumpDelegate();
    public event JumpDelegate OnJump;
    public event JumpDelegate OnJumpHeld;
    public delegate void ShootDelegate();
    public event ShootDelegate OnShoot;


    public void MoveInput(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }


    public void LookInput(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }


    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (OnJump != null) OnJump();
        }
        else if (context.started)
        {
            jumpHeld = true;
        }
        else if (context.canceled)
        {
            jumpHeld = false;
        }
    }


    public void SprintInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            sprint = true;
        }
        else if (context.canceled)
        {
            sprint = false;
        }
    }


    public void ShootInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (OnShoot != null) OnShoot();
        }
    }
}
