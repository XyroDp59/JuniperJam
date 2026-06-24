using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider), typeof(AttributSet))]
public class Beyblade : Projectile
{
    [SerializeField] int attackPower = 10;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float minSpeed = .1f;
    [SerializeField] float desceleration = 3f;
    [SerializeField] float speedBonusOnCollision = 1f;
    [SerializeField] float speedBonusOnHit = 2.5f;
    [SerializeField] float lifetime = 30;

    [SerializeField] float targetCircleSize = 6;
    [SerializeField] float arenaSize = 12f;

    AttributSet health;
    Rigidbody rb;
    Vector3 currentDir;
    float currentSpeed;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<AttributSet>();

        rb.maxLinearVelocity = maxSpeed;
        currentSpeed = maxSpeed / 2f;
        currentDir = direction;

        health.onDeath.AddListener(KillBeyblade);
        health.onHpChange.AddListener(OnHit);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<RotatingArena>(out var arena)) return;

        currentDir = ChooseNextDirection(speedBonusOnCollision);
        Debug.Log($"Maxwell, go to {currentDir} !");
        
        // deal damage
        if (other.gameObject != player.gameObject
            && other.TryGetComponent<AttributSet>(out AttributSet health))
        {
            health.CurrentHp -= (int) (attackPower * currentSpeed/maxSpeed);
        }
    }

    private Vector3 ChooseNextDirection(float speedBonus)
    {
        currentSpeed += speedBonus;
        Vector3 preferredDir = Random.onUnitCircle;
        preferredDir = new Vector3(preferredDir.x, 0, preferredDir.y);

        Vector2 posOnCircle = Random.onUnitCircle * targetCircleSize;
        Vector3 target = new Vector3(posOnCircle.x, 0, posOnCircle.y);
        target = (target - transform.position).normalized;

        return Vector3.Lerp(preferredDir, target, transform.position.magnitude / arenaSize).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = currentDir * currentSpeed;
        currentSpeed -= desceleration * Time.fixedDeltaTime;

        if(currentSpeed < minSpeed) KillBeyblade();
    }

    private void KillBeyblade()
    {
        Destroy(gameObject);
    }

    private void OnHit(int currentHP, float healthPercentage)
    {
        ChooseNextDirection(speedBonusOnHit);
    }
}

