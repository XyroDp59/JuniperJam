using UnityEngine;

public class HorseScript : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 3;
    [SerializeField] private float dashSpeed = 20;
    [SerializeField] private float dashTimer = 3;
    [SerializeField] private float dashDistance = 2;
    [SerializeField] private GameObject player;

    private float timer = 0;
    private Vector3 savePlayerPosition = Vector3.zero;
    private bool isDashing = false;
    private float playerX;
    private float playerZ;

    private void Awake()
    {
        //BREAKING CHANGE: change that line if the name of the player is different than the one from the prefab
        player = GameObject.Find(player.name);
    }

    void Update()
    {
        if (player.activeSelf && !isDashing)
        {
            Debug.Log("Hello");
            PlayerCoordinateUpdate(playerX, playerZ);
            transform.position += getEnnemiDirection(player.transform.position, transform.position) * baseSpeed * Time.deltaTime;
            if (Vector3.Distance(savePlayerPosition, transform.position) < dashDistance)
            {
                isDashing = true;
                savePlayerPosition = new Vector3(playerX, 0, playerZ);
            }
        }
        else if (player.activeSelf && isDashing)
        {
            if (timer > dashTimer)
            {
                transform.position += getEnnemiDirection(savePlayerPosition, transform.position) * dashSpeed * Time.deltaTime;
                //Debug.Log(Vector3.Distance(savePlayerPosition, transform.position) < 0.7);
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
        else
        {
            transform.position = Vector3.zero;          //ennemi don't move if the player is inactive
        }
    }

    private void PlayerCoordinateUpdate(float x, float z)
    {
        x = getPlayerX();
        z = getPlayerZ();
    }

    private float getPlayerX()
    {
        return player.transform.position.x;
    }

    private float getPlayerZ()
    {
        return player.transform.position.z;
    }

    private Vector3 getEnnemiDirection(Vector3 playerPosition, Vector3 ennemiPosition)
    {
        Vector3 ennemiDirection = playerPosition - ennemiPosition;
        ennemiDirection.y = 0;                                         //ennemi cannot jump
        return Vector3.Normalize(ennemiDirection);
    }
}

