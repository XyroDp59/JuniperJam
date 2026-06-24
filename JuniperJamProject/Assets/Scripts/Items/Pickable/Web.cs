using UnityEngine;

public class Web : Projectile
{
    [SerializeField, Range(0,1)] float slownessFactor = 0.2f;

    private void Awake()
    {
        transform.parent = RotatingArena.Singleton.transform;
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
