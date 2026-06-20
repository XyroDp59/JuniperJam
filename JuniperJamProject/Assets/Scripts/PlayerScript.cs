using System.Collections;
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
    
    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashDistance = 2;
    [SerializeField] private float dashInvulnerableTime = 0.5f;
    
    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private Vector2 lastMoveInput;
    private bool dashing = false;
    
    [Header("Abilities")]
    [SerializeField] private int basicAttackDamage = 10;
    [SerializeField] private float basicAttackSize = 1;
    [SerializeField] private float basicAttackDuration = 1;
    [SerializeField] private BasicAttack basicAttackPrefab;
    private BasicAttack basicAttackObject;
    private bool isAttacking = false;
    //[SerializeField] private float basicAttackParticule = 10;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); lastMoveInput = moveInput; };
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Player.Sprint.performed += ctx => StartCoroutine(Dash());
        controls.Player.Attack.performed += ctx => StartCoroutine(BasicAttack());
        
        basicAttackObject = Instantiate(basicAttackPrefab); basicAttackObject.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (dashing)
        {
            rb.MovePosition(transform.position + new Vector3(lastMoveInput.x * dashSpeed * Time.fixedDeltaTime, 0, lastMoveInput.y * dashSpeed * Time.fixedDeltaTime));
            return;
        }
        if (moveInput != Vector2.zero)
            rb.MovePosition(transform.position + new Vector3(moveInput.x * speed * Time.fixedDeltaTime, 0, moveInput.y * speed * Time.fixedDeltaTime));
    }

    private IEnumerator Dash()
    {
        if (!dashing && !isAttacking)
        {
            dashing = true;
            yield return new WaitForSeconds(dashDistance / dashSpeed);
            dashing = false;
        }
    }

    private IEnumerator BasicAttack()
    {
        if (!dashing && !isAttacking)
        {
            isAttacking = true;
            
            basicAttackObject.damage = basicAttackDamage; 
            basicAttackObject.timeToLive = 0.25f;
            basicAttackObject.transform.position = transform.position; 
            basicAttackObject.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(basicAttackDuration);
            isAttacking = false;
        }
    }
}
