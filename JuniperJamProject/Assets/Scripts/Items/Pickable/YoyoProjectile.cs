using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class YoyoProjectile : BaseMovingItem
{
    [SerializeField] private Rigidbody rb;
    
    [HideInInspector] public int damage;
    [HideInInspector] public float dashSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public float dashDistance;
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public float approchingSpeed;
    [SerializeField] private Transform stringTransform;
    
    private bool _isDashing = true;
    private float _currentDashDistance;
    private float _fixedUpdateDashDistance;
    private float _currentMaxDistance;

    private void Awake()
    {
        _fixedUpdateDashDistance = dashSpeed * Time.fixedDeltaTime;
        _currentMaxDistance = dashDistance;
        //print("damage : " + damage + "; dashSpeed : " + dashSpeed + "; speed : " + speed + "; dashDistance : " + dashDistance + "; dashDirection : " + dashDirection + "; approchingSpeed : " + approchingSpeed + "; _fixedUpdateDashDistance : " + _fixedUpdateDashDistance);
    }

    private void Update()
    {
        if (!_isDashing)
        {
            _currentMaxDistance -= approchingSpeed * Time.deltaTime;
            if (_currentMaxDistance <= 1f) Destroy(gameObject);
        }

        Vector3 dir = (player.transform.position - transform.position) / 2;
        stringTransform.position = transform.position + dir + 0.5f * Vector3.up;
        stringTransform.rotation = Quaternion.LookRotation(Vector3.up, dir);
        stringTransform.localScale = new Vector3(0.05f, dir.magnitude, 0.05f);
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            rb.MovePosition(transform.position + dashDirection * _fixedUpdateDashDistance);
            _currentDashDistance += _fixedUpdateDashDistance;
            if (_currentDashDistance >= dashDistance) _isDashing = false;
        }
        else
        {
            Vector2 input = GetInput();
            Vector3 playerPos = player.transform.position;
            Vector3 Target;
            if (isKeyboardOrMouse)
            {
                Vector3 dirFromPlayer = new Vector3(input.x - playerPos.x, 0, input.y - playerPos.z);
                if (dirFromPlayer.magnitude > _currentMaxDistance)
                    Target = playerPos + dirFromPlayer.normalized * _currentMaxDistance;
                else
                    Target = new Vector3(input.x, 0, input.y);
            }
            else
            {
                Target = playerPos + new Vector3(input.x * _currentMaxDistance, 0, input.y * _currentMaxDistance);
            }
    
            if ((Target - transform.position).magnitude >= speed * Time.fixedDeltaTime)
                Target = transform.position + (Target - transform.position).normalized * (speed * Time.fixedDeltaTime);
            
            rb.MovePosition(Target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.gameObject.TryGetComponent(out AttributSet attributSet))
        {
            attributSet.CurrentHp -= damage;
        }
    }
}
