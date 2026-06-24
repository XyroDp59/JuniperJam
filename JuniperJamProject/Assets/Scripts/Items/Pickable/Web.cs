using UnityEngine;

public class Web : Projectile
{
    [SerializeField, Range(0,1)] float slownessFactor = 0.2f;
    [SerializeField, Range(0.5f, 1000f)] float lifetime = 30f;

    private void Awake()
    {
        transform.parent = RotatingArena.Singleton.transform;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnnemiClassScript>(out var ennemi))
        {
            ennemi.slownessFactor = slownessFactor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EnnemiClassScript>(out var ennemi))
        {
            ennemi.slownessFactor = 1;
        }
    }
}
