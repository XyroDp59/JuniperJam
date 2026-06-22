using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseMovingItem : Item
{
    private bool isKeyboardOrMouse = false;
    private Camera cam;
    private Mouse mouse;
    private Vector2 moveInput;

    private void OnEnable()
    {
        player.controls.Player.Move.performed += (context) => {
            if ( context.control.device is Keyboard or Mouse ) {isKeyboardOrMouse = true;
                mouse = Mouse.current;
            }
            else isKeyboardOrMouse = false;
        };
        player.controls.Player.ItemControl.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        player.controls.Player.ItemControl.canceled += ctx => moveInput = Vector2.zero;
        
        cam = Camera.main; //ou main camera ?
        
        player.controls.Player.Attack.performed += ctx => GetDirection();
    }

    private Vector2 GetDirection()
    {
        if (isKeyboardOrMouse)
        {
            Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue()); // a optimiser
            Physics.Raycast(ray, out RaycastHit hit);
            print (hit.point);
            Vector3 dir = hit.point - transform.position;
            return new Vector2(dir.x, dir.z).normalized; 
        }
        
        print(moveInput);
        return moveInput;
    }
}
