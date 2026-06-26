using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnerList = new List<GameObject>();
    [SerializeField] private List<SpawnerScript> spawnerScriptList = new List<SpawnerScript>();
    [SerializeField] private float spawnRate = 5;
    private GameObject player;
    private List<GameObject> canSpawnList = new List<GameObject>();
    private float gameTimer = 0;
    private float spawnTimer = 0;
    private int random = 0;

    void Start()
    {
        gameTimer = 0;
        for (int i = 0; i < spawnerList.Count; i++)
        {
            spawnerScriptList[i] = spawnerList[i].GetComponent<SpawnerScript>();
        }
        spawnTimer = 0;
    }

    void Update()
    {
        gameTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            spawnTimer = 0;
            for (int i = 0; i < spawnerList.Count; i++)
            {
                if (spawnerScriptList[i].getCanSpawn(gameTimer) && !canSpawnList.Contains(spawnerList[i]))
                {
                    for (int j = 0; j < spawnerScriptList[i].getSpawnPercentage(); j++)
                    {
                        canSpawnList.Add(spawnerList[i]);
                    }
                }
                if (gameTimer >= 60 && spawnerScriptList[i].getSpawnPercentage() != spawnerScriptList[i].getFinalSpawnPercentage())
                {
                    int saveSpawn = spawnerScriptList[i].getSpawnPercentage();
                    spawnerScriptList[i].changeSpawnPercentage();
                    int check = saveSpawn - spawnerScriptList[i].getSpawnPercentage();
                    if (check < 0)
                    {
                        for (int j = 0; j < -check; j++)
                        {
                            canSpawnList.Add(spawnerList[i]);
                        }
                    }
                    else if (check > 0)
                    {
                        for (int j = 0; j < check; j++)
                        {
                            canSpawnList.Remove(spawnerList[i]);
                        }
                    }
                }
            }
            for (int i = 0; i < Mathf.Log(gameTimer + 1); i++)
            {
                // Debug.Log(Mathf.Log(gameTimer + 1));
                random = Random.Range(0, canSpawnList.Count - 1);
                canSpawnList[random].GetComponent<SpawnerScript>().spawn();
            }
        }
        spawnTimer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player = other.gameObject;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
