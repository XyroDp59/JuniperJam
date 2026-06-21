using UnityEngine;

public class BomberScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private SpawnerScript bomberSpawner;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float speedY;
    [SerializeField] private float timeOfFly;

    private bool isDescending = false;
    private bool isAscending = false;
    private float timer = 0;
    private float angle;

    void Awake()
    {
        Debug.Log(transform.position);
        timer = 0;
        isDescending = true;
        isAscending = false;
        bomberSpawner = GameObject.FindGameObjectWithTag("BomberSpawner").GetComponent<SpawnerScript>();
        angle = bomberSpawner.getAngle();
    }


    void Update()
    {
        timer += Time.deltaTime * speed;
        if (!isDescending && !isAscending)
        {
            if (timer > timeOfFly)
            {
                isAscending = true;
            }
            transform.position = new Vector3(Mathf.Cos(angle + timer) * bomberSpawner.getCarrouselRadius(),
                                            transform.position.y,
                                            Mathf.Sin(angle + timer) * bomberSpawner.getCarrouselRadius());
        }
        else if (isDescending)
        {
            Debug.Log(maxY - (timer * speedY));
            transform.position = new Vector3(Mathf.Cos(angle + timer) * bomberSpawner.getCarrouselRadius(),
                                            maxY - timer * speedY,
                                            Mathf.Sin(angle + timer) * bomberSpawner.getCarrouselRadius());
            if (transform.position.y < minY)
            {
                isDescending = false;
                angle = angle + timer;
                timer = 0;
            }
        }
        else if (isAscending)
        {
            transform.position = new Vector3(Mathf.Cos(angle + timer) * bomberSpawner.getCarrouselRadius(),
                                            minY + (timer - timeOfFly)*speedY,
                                            Mathf.Sin(angle + timer) * bomberSpawner.getCarrouselRadius());
            if (transform.position.y > maxY)
            {
                Destroy(gameObject);
            }
        }
    }

    public float getMaxY()
    {
        return maxY;
    }
}