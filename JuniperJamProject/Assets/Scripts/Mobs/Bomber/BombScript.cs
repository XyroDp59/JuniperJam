using UnityEngine;

public class BombScript : EnnemiClassScript
{
    [SerializeField] private GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
