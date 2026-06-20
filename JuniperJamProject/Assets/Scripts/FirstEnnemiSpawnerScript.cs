using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class FirstEnnemiSpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject firstEnnemi;
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private float carrouselRadius = 8;
    [SerializeField] private int nombreSpawnPossible = 100000;

    private float timer = 0;

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Instantiate(firstEnnemi, getRandomSpawn(), transform.rotation);
        }
    }

    private Vector3 getRandomSpawn()
    {
        float angle = UnityEngine.Random.Range(0, nombreSpawnPossible) * 2 * math.PI / nombreSpawnPossible;
        Vector3 spawnPosition = new Vector3(math.cos(angle), 0, math.sin(angle)) * carrouselRadius;
        return spawnPosition;
    }
}
