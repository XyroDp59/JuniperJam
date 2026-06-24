using System.Threading;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float explosionTime;
    [SerializeField] private int damage;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out AttributSet attributSet) && Vector3.SqrMagnitude(transform.position - other.gameObject.transform.position) < 1.5)
        {
            attributSet.CurrentHp -= damage;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out AttributSet attributSet) && Vector3.SqrMagnitude(transform.position - other.gameObject.transform.position) < 1.5)
        {
            attributSet.CurrentHp -= damage;
        }
    }
}
