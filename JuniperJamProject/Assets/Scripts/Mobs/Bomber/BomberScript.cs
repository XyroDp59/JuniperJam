using UnityEngine;

public class BomberScript : EnnemiClassScript
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private SpawnerScript bomberSpawner;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float speedY;
    [SerializeField] private float timeOfFly;

    [Header("Bombs")]
    [SerializeField] private GameObject bomb;
    [SerializeField] private float bombRate;

    private float bomberPathRadius;
    private bool isDescending = false;
    private bool isAscending = false;
    private float timer = 0;
    private float lastTimeBomb = 0;
    private float angle;
    
    // SFX
    private FMOD.Studio.EventInstance planeInstance;

    void Awake()
    {
        timer = 0;
        isDescending = true;
        isAscending = false;
        bomberSpawner = GameObject.FindGameObjectWithTag("BomberSpawner").GetComponent<SpawnerScript>();
        angle = bomberSpawner.getAngle();
        bomberPathRadius = bomberSpawner.getCarrouselRadius();
        
        //SFX
        planeInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Game/Plane");
        planeInstance.start();
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
            transform.position = new Vector3(cos(angle + timer),
                                            transform.position.y,
                                            sin(angle + timer));
            if (timer - lastTimeBomb > bombRate)
            {
                lastTimeBomb = timer;
                Instantiate(bomb,
                            new Vector3(transform.position.x, transform.position.y - (1/2), transform.position.z),
                            transform.rotation);
            }
        }
        else if (isDescending)
        {
            transform.position = new Vector3(cos(angle + timer),
                                            maxY - timer * speedY,
                                            sin(angle + timer));
            if (transform.position.y < minY)
            {
                isDescending = false;
                angle = angle + timer;
                timer = 0;
            }
        }
        else if (isAscending)
        {
            transform.position = new Vector3(cos(angle + timer),
                                            minY + (timer - timeOfFly) * speedY,
                                            sin(angle + timer));
            if (transform.position.y > maxY)
            {
                Destroy(gameObject);
                
                // SFX
                planeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    private float cos(float angle)
    {
        return Mathf.Cos(angle) * bomberPathRadius;
    }

    private float sin(float angle)
    {
        return Mathf.Sin(angle) * bomberPathRadius;
    }

    public float getMaxY()
    {
        return maxY;
    }
}