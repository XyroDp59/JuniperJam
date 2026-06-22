using System.Threading;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float explosionTime;

    private float timer = 0;

    private void Awake()
    {
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > explosionTime)
        {
            Destroy(gameObject);
        }
    }
}
