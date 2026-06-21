using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject ennemi;
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private float carrouselRadius = 8;
    [SerializeField] private int nombreSpawnPossible = 100000;

    private float timer = 0;
    private float angle;

    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            randomAngle();
            Instantiate(ennemi, spawn(), transform.rotation);
        }
    }

    private Vector3 spawn()
    {
        float y = 0;
        if (ennemi.TryGetComponent<BomberScript>(out BomberScript bomber))
        {
            y = bomber.getMaxY();
        }
        Vector3 spawnPosition = new Vector3(math.cos(angle) * carrouselRadius, y, math.sin(angle) * carrouselRadius);
        return spawnPosition;
    }

    private void randomAngle()
    {
        angle = UnityEngine.Random.Range(0, nombreSpawnPossible) * 2 * math.PI / nombreSpawnPossible;
    }

    public float getAngle()
    {
        return angle;
    }

    public float getCarrouselRadius()
    {
        return carrouselRadius;
    }
}
