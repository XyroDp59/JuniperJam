using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseMovingItem : MonoBehaviour
{
    [HideInInspector] public PlayerScript player;
    
    protected bool isKeyboardOrMouse = true;
    private Camera cam;
    private Vector2 moveInput;

    private void OnEnable()
    {
        player.controls.Player.Move.performed += (context) => {
            if ( context.control.device is Keyboard or Mouse ) isKeyboardOrMouse = true;
            else if (context.ReadValue<Vector2>() != Vector2.zero)
                isKeyboardOrMouse = false;
        };
        
        player.controls.Player.ItemControl.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
            if (moveInput != Vector2.zero) 
                isKeyboardOrMouse = false;
        };
        player.controls.Player.ItemControl.canceled += ctx => moveInput = Vector2.zero;
    }

    protected Vector2 GetInput()
    {
        if (isKeyboardOrMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // a optimiser
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            plane.Raycast(ray, out float hit);
            Vector3 point = ray.GetPoint(hit);
            return new Vector2(point.x, point.z);
            //Vector3 dir = hit.point - transform.position;
            //return new Vector2(dir.x, dir.z).normalized; 
        }
        
        return moveInput;
    }
}
