using UnityEngine;
using UnityEngine.AI;

public class ChildrenScript : EnnemiClassScript
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed = 3;

    private GameObject player = null;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        if (player != null && player.activeSelf)
        {
            agent.SetDestination(player.transform.position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player = other.gameObject;
        }
    }
}
