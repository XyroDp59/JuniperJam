using UnityEngine;

public class FirstEnnemiScript : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private GameObject player;

    private float playerX;
    private float playerZ;

    private void Awake()
    {
        //BREAKING CHANGE: change that line if the name of the player is different than the one from the prefab
        player = GameObject.Find(player.name);
    }

    void Update()
    {
        if (player.activeSelf)
        {
            PlayerCoordinateUpdate(playerX, playerZ);
            transform.position += getEnnemiDirection(player.transform.position, transform.position) * speed * Time.deltaTime;
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
