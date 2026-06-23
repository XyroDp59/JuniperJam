using UnityEngine;
using UnityEngine.AI;

public class ChildrenScript : EnnemiClassScript
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed = 3;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        //BREAKING CHANGE: change that line if the name of the player is different than the one from the prefab
        player = GameObject.Find(player.name);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        if (player.activeSelf)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}
