using System;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [SerializeField] private EnnemiClassScript ennemi;
    [SerializeField] private int minRadius = 6;
    [SerializeField] private int maxRadius = 10;
    [SerializeField] private int nombreSpawnPossible = 1000;
    [SerializeField] private float distanceMinFromPlayer = 5;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] public int timeBeforeCanSpawn;
    [SerializeField] public int initialSpawnPercentage = 0;
    [SerializeField] public int finalSpawnPercentage = 0;
    [SerializeField] public int spawnPercentageChange;
    [SerializeField] public MobSpawnerScript mobSpawner;

    private int carrouselRadius;
    private GameObject player;
    private int spawnPercentage;
    private float angle;

    private void Start()
    {
        carrouselRadius = UnityEngine.Random.Range(minRadius, maxRadius);
        _scoreManager = _scoreManager.GetComponent<ScoreManager>();
        Debug.Log(_scoreManager);
        mobSpawner = mobSpawner.GetComponent<MobSpawnerScript>();
        player = mobSpawner.GetPlayer();
        spawnPercentage = initialSpawnPercentage;
    }

    public void spawn()
    {
        randomAngle();
        Debug.Log(Vector3.Distance(spawnPosition(), player.transform.position));
        EnnemiClassScript ennemiInstance = Instantiate(ennemi, spawnPosition(), transform.rotation);
        ennemiInstance.attributSet.onDeath.AddListener(() => { Destroy(ennemiInstance.gameObject, 5f); ennemiInstance.slownessFactor = 0f; });
        ennemiInstance.attributSet.onDeath.AddListener(() => { _scoreManager.IncrementScore(ennemiInstance.rewardScore); }); //jsp pk on peut pas le faire depuis l'ennemi ca
    }

    private Vector3 spawnPosition()
    {
        Vector3 spawnPosition = new Vector3(math.cos(angle) * carrouselRadius, 0, math.sin(angle) * carrouselRadius);
        return spawnPosition;
    }

    private void randomAngle()
    {
        angle = UnityEngine.Random.Range(0, nombreSpawnPossible) * 2 * math.PI / nombreSpawnPossible;
        while (Vector3.Distance(spawnPosition(), player.transform.position) < distanceMinFromPlayer)
        {
            angle = UnityEngine.Random.Range(0, nombreSpawnPossible) * 2 * math.PI / nombreSpawnPossible;
        }
    }

    public float getAngle()
    {
        return angle;
    }

    public float getCarrouselRadius()
    {
        return carrouselRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player = other.gameObject;
        }
    }

    public bool getCanSpawn(float time)
    {
        return (timeBeforeCanSpawn <= time);
    }

    public int getSpawnPercentage()
    {
        return spawnPercentage;
    }

    public void changeSpawnPercentage()
    {
        spawnPercentage = spawnPercentage + spawnPercentageChange;
    }

    public int getFinalSpawnPercentage()
    {
        return finalSpawnPercentage;
    }
}
