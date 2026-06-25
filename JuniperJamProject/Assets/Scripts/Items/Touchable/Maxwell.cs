using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Maxwell : MonoBehaviour
{
    [SerializeField] int attackPower = 100;
    [SerializeField] float speed = 10f;
    [SerializeField] string tag = "Wall";
    [SerializeField] float maxwellDuration = 5f;
    [SerializeField] float targetCircleSize = 3f;

    Vector3 currentDir;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public PlayerScript player;


    private void ActivateMaxwell(bool b)
    {
        player.GetMesh().gameObject.SetActive(!b);
        player.TogglePlayerInput(!b);
        player.GetComponent<CapsuleCollider>().enabled = !b;
    }

    private void Start()
    {
        ActivateMaxwell(true);

        currentDir = new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f,1.0f)).normalized;
        if (currentDir == Vector3.zero) currentDir = Vector3.left;

        StartCoroutine(MaxwellDeath());
    }

    IEnumerator MaxwellDeath()
    {
        yield return new WaitForSeconds(maxwellDuration);
        ActivateMaxwell(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            currentDir = ChooseNextDirection();
            Debug.Log($"Maxwell, go to {currentDir} !");
        }
        else if (other.gameObject != player.gameObject 
            && other.TryGetComponent<AttributSet>(out AttributSet health))
        {
            health.CurrentHp -= attackPower;
        }
    }

    private Vector3 ChooseNextDirection()
    {
        Vector2 posOnCircle = Random.onUnitCircle * targetCircleSize;
        Vector3 target = new Vector3(posOnCircle.x, 0, posOnCircle.y);
        return (target - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        playerRb.linearVelocity = currentDir * speed;
    }

}
