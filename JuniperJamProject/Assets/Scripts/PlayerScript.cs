using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody rb;

    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float speed = 10;
    [SerializeField] private LayerMask groundLayer;



    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Jump.performed += ctx =>
        {
            if (IsGrounded())
            {
                rb.linearVelocity += Vector3.up * jumpForce;
            }
        };
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.y * speed);
    }


    bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.15f, groundLayer, QueryTriggerInteraction.Ignore);
    }
}
