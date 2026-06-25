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

    [Header("Visuals")]
    [SerializeField] private Animator animator;
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    [SerializeField] private AnimatorToMaterial mesh;
    
    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashDistance = 2;
    [SerializeField] private float dashInvulnerableTime = 0.5f;
    
    [HideInInspector] public InputSystem_Actions controls;
    private Coroutine movingCoroutine;
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

    PickableItem weaponA;
    PickableItem weaponB;
    [HideInInspector] public PickableItem itemToAssign;
    bool releaseItem;



    #region Inputs and setup

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => { movingCoroutine = StartCoroutine(StartMoving(ctx)); }; 
        controls.Player.Move.canceled += ctx => StopMoving(ctx); 
        controls.Player.Sprint.performed += ctx => StartCoroutine(Dash());
        controls.Player.Attack.performed += ctx => StartCoroutine(BasicAttack());
        
        basicAttackObject = Instantiate(basicAttackPrefab); basicAttackObject.gameObject.SetActive(false);

        // Items
        controls.Player.Release.performed += ctx => releaseItem = true;
        controls.Player.Release.canceled += ctx => releaseItem = false;
        controls.Player.ItemA.started += ctx => PickableHandler(ctx, ref weaponA); 
        controls.Player.ItemB.started += ctx => PickableHandler(ctx, ref weaponB);
    }


    void OnDisable() 
    { 
        controls.Disable();
        if(movingCoroutine != null) StopCoroutine(movingCoroutine);
    }
    public void TogglePlayerInput(bool b)
    {
        if (controls.UI.enabled)
        {
            Debug.LogError("[TogglePlayerInput] are you sure you want to toggle manually the player when in UI ?");
        }

        if (b) controls.Player.Enable();
        else controls.Player.Disable();
    }

    public void TogglePlayerMovement(bool b)
    {
        if (b) controls.Player.Move.Enable();
        else controls.Player.Move.Disable();
    }

    #endregion

    #region Movement
    private IEnumerator StartMoving(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>(); 
        lastMoveInput = moveInput;

        while (moveInput != Vector2.zero)
        {
            rb.MovePosition(transform.position + new Vector3(moveInput.x * speed * Time.fixedDeltaTime, 0, moveInput.y * speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }
    }
    private void StopMoving(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
        StopCoroutine(movingCoroutine);
        movingCoroutine = null;
    }


    private IEnumerator Dash()
    {
        if (!dashing && !isAttacking)
        {
            dashing = true;
            float timer = 0;
            while(timer < dashDistance / dashSpeed)
            {
                rb.linearVelocity = new Vector3(lastMoveInput.x * dashSpeed, 0, lastMoveInput.y * dashSpeed);
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }
            dashing = false;
        }
    }

    public Vector2 GetMoveDirection()
    {
        return lastMoveInput;
    }

    public bool IsDashing()
    {
        return dashing;
    }

    #endregion

    #region Items
    private void PickableHandler(InputAction.CallbackContext context, ref PickableItem weapon)
    {
        if (releaseItem)
        {
            weapon = null;
            // todo : update UI
            return;
        }

        if (weapon != null && weapon.isItemActive)
        {
            weapon.Use();
        }
        else if(itemToAssign != null)
        {
            itemToAssign.PickUp(ref weapon, transform);
        }
        // todo : update UI
    }
    #endregion 

    private IEnumerator BasicAttack()
    {
        if (!dashing && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool(IsAttacking, true);
            
            basicAttackObject.damage = basicAttackDamage; 
            basicAttackObject.timeToLive = 0.25f;
            basicAttackObject.transform.position = transform.position; 
            basicAttackObject.gameObject.SetActive(true);
            
            mesh.Spin(0.8f * basicAttackDuration, 1);
            
            yield return new WaitForSeconds(basicAttackDuration);
            isAttacking = false;
            animator.SetBool(IsAttacking, false);
        }
    }

    public AnimatorToMaterial GetMesh()
    {
        return mesh;
    }

}
