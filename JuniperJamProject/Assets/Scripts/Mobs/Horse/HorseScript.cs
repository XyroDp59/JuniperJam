using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class HorseScript : EnnemiClassScript
{
    Rigidbody rb;

    [SerializeField] private float baseSpeed = 3;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20;
    [SerializeField] private float dashTimer = 3;
    [SerializeField] private float dashDistance = 2;

    private float timer = 0;
    private Vector3 savePlayerPosition = Vector3.zero;
    private GameObject player = null;
    private bool isDashing = false;
    private float playerX;
    private float playerZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (player != null && player.activeSelf && !isDashing)
        {
            playerX = getPlayerX();
            playerZ = getPlayerZ();
            rb.MovePosition(transform.position + getDirection(player.transform.position) * Time.deltaTime * baseSpeed * slownessFactor);
            if (Vector3.Distance(player.transform.position, transform.position) < dashDistance)
            {
                isDashing = true;
                savePlayerPosition = new Vector3(playerX, 0, playerZ);
                
                // SFX
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Horse");
            }
        }
        else if (player != null && player.activeSelf && isDashing)
        {
            if (timer > dashTimer)
            {
                rb.MovePosition(transform.position + getDirection(savePlayerPosition) * Time.deltaTime * dashSpeed * slownessFactor);
                if (Vector3.Distance(savePlayerPosition, transform.position) < 0.7)
                {
                    timer = 0;
                    isDashing = false;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private float getPlayerX()
    {
        return player.transform.position.x;
    }

    private float getPlayerZ()
    {
        return player.transform.position.z;
    }

    private Vector3 getDirection(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0;
        direction.Normalize();
        return direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player = other.gameObject;
        }
    }
}

