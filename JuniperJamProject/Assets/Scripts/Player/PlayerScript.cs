using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    // 2. These variables are to hold the Action references
    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody rb;
    
    [SerializeField] private float invulnerabilityTime = 0.5f;
    [SerializeField] private AttributSet attributSet;

    [Header("Visuals")]
    [SerializeField] private Animator animator;
    public static readonly int IsAttackingAnimator = Animator.StringToHash("IsAttacking");
    private static readonly int IsDashingAnimator = Animator.StringToHash("IsDashing");
    private static readonly int IsHurtAnimator = Animator.StringToHash("IsDashing");
    [SerializeField] private AnimatorToMaterial mesh;
    [SerializeField] private GameObject itemArrow;
    
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

    [SerializeField] private Image weaponAUI;
    [SerializeField] private Image weaponBUI;
    PickableItem weaponA;
    PickableItem weaponB;
    private PickableItem itemToAssign;
    bool releaseItem;



    #region Inputs and setup

    public InputSystem_Actions GetInputActions()
    {
        return controls;
    }

    private void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        itemArrow.SetActive(false);
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
        controls.Player.ItemA.started += ctx => PickableHandler(ctx, ref weaponA, ref weaponAUI); 
        controls.Player.ItemB.started += ctx => PickableHandler(ctx, ref weaponB, ref weaponBUI);
        attributSet.onHpChange.AddListener((int hpChange, int _, float _) =>
        {
            if (hpChange < 0) StartCoroutine(BecomeInvulnerable());
        });
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
            Vector2 dir = lastMoveInput;
            
            dashing = true;
            animator.SetBool(IsDashingAnimator, true);
            float timer = 0;
            attributSet.invulnerable = true;
            while(timer < dashDistance / dashSpeed)
            {
                rb.linearVelocity = new Vector3(dir.x * dashSpeed, 0, dir.y * dashSpeed);
                yield return new WaitForFixedUpdate();
                timer += Time.fixedDeltaTime;
            }
            dashing = false;
            attributSet.invulnerable = false;
            animator.SetBool(IsDashingAnimator, false);
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

    public PickableItem GetItemToAssign()
    {
        return itemToAssign;
    }

    public void SetItemToAssign(PickableItem item)
    {
        itemToAssign = item;

        if(item == null)
        {
            itemArrow.SetActive(false);
            itemArrow.transform.parent = transform.parent;
        }
        else
        {
            itemArrow.SetActive(true);
            itemArrow.transform.parent = item.transform;
        }
        itemArrow.transform.localPosition = Vector3.zero;
    }


    private void PickableHandler(InputAction.CallbackContext context, ref PickableItem weapon, ref Image image)
    {
        if (releaseItem)
        {
            weapon = null;
            image.gameObject.SetActive(false);
            image.sprite = null;

            return;
        }

        if (weapon != null && weapon.isItemActive)
        {
            weapon.Use();
            if (!weapon.isItemActive) image.gameObject.SetActive(false);
            //print(weapon.isItemActive);
        }
        else if(itemToAssign != null)
        {
            itemToAssign.PickUp(ref weapon, transform);
            if (weapon.isItemActive)
            {
                print(itemToAssign);
                image.sprite = itemToAssign.sprite;
                image.gameObject.SetActive(true);
                SetItemToAssign(null);
                // SFX
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Pickup");
            }
        }
        // todo : update UI
    }
    #endregion 

    private IEnumerator BasicAttack()
    {
        if (!dashing && !isAttacking)
        {
            // ----- SFX  ----
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Woosh");
            // ---------------
            
            isAttacking = true;
            animator.SetBool(IsAttackingAnimator, true);
            
            basicAttackObject.damage = basicAttackDamage; 
            basicAttackObject.timeToLive = 0.25f;
            basicAttackObject.transform.position = transform.position; 
            basicAttackObject.gameObject.SetActive(true);
            
            mesh.Spin(0.8f * basicAttackDuration, 1);
            
            yield return new WaitForSeconds(basicAttackDuration);
            isAttacking = false;
            animator.SetBool(IsAttackingAnimator, false);
        }
    }

    public AnimatorToMaterial GetMesh()
    {
        return mesh;
    }

    private IEnumerator BecomeInvulnerable()
    {
        attributSet.invulnerable = true;
        animator.SetBool(IsHurtAnimator, true);
        yield return new WaitForSeconds(invulnerabilityTime);
        animator.SetBool(IsHurtAnimator, false);
        attributSet.invulnerable = false;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
