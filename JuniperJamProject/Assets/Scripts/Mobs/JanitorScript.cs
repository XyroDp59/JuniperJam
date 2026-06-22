using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class JanitorScript : EnnemiClassScript
{
    Rigidbody rb;
    [SerializeField] public NavMeshAgent agent;

    [SerializeField] private float speed;
    [SerializeField] private GameObject player;

    [Header("Prevent against OOB")]
    [SerializeField] private float carrouselRadius;
    [SerializeField] private float safeZoneRadius;
    [SerializeField] private GameObject arena;

    [Header("Items")]
    [SerializeField] private ItemSpawner itemSpawner;
    [SerializeField] private float timeToWaitBeforeChassingItem;

    [Header("Flee")]
    [SerializeField] private float fleeDistance;
    [SerializeField] private float fleeOverItemDistance;

    [Header("Random Movement")]
    [SerializeField] private int minRandomMovementTime;
    [SerializeField] private int maxRandomMovementTime;

    private bool isReturningToSafeZone = false;
    private bool isItemOnArena = false;
    private float timerForItem = 0;
    List<Item> items = new List<Item>();
    Item itemTarget = null;
    private GameObject itemHeld = null;
    private float distanceToPlayer = 0;
    private float timerRandomMovement = 0;
    private float randomTimeMovement = 0;
    private Vector3 randomPosition = Vector3.zero;

    void Awake()
    {
        //BREAKING CHANGE: change that line if the name of the player is different than the one from the prefab
        player = GameObject.Find(player.name);
        //BREAKING CHANGE: change that line if the name of the arena is different than the one from the prefab
        arena = GameObject.Find(arena.name);

        isReturningToSafeZone = false;
        itemSpawner = GameObject.FindGameObjectWithTag("ItemSpawner").GetComponent<ItemSpawner>();
        timerForItem = 0;
        isItemOnArena = false;
        itemHeld = null;
        randomTimeMovement = Random.Range(minRandomMovementTime, maxRandomMovementTime);
        items = itemSpawner.getActiveItems();
        randomPosition = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        items = itemSpawner.getActiveItems();
        isItemOnArena = items.Count != 0;
        if (items.Count == 1 && itemTarget != items[0])
        {
            timerForItem = 0;
            itemTarget = items[0];
        }
        if (isItemOnArena && timerForItem > timeToWaitBeforeChassingItem)
        {
            if (distanceToPlayer < fleeOverItemDistance)
            {
                FleeMovement();
            }
            else
            {
                ToItemMovement();
            }
            timerRandomMovement = 0;
        }
        else
        {
            if (distanceToPlayer < fleeDistance)
            {
                FleeMovement();
                timerRandomMovement = 0;
            }
            else
            {
                RandomMovement();
            }
            if (isItemOnArena)
            {
                timerForItem += Time.deltaTime;
            }
        }
    }

    private void FleeMovement()
    {
        if (Vector3.Distance(transform.position, arena.transform.position) >= carrouselRadius)
        {
            Vector3 positionPlus = new Vector3(transform.position.x * cos(Time.deltaTime * speed)
                                                - transform.position.z * sin(Time.deltaTime * speed),
                                            0,
                                            transform.position.z * cos(Time.deltaTime * speed)
                                                + transform.position.x * sin(Time.deltaTime * speed));
            Vector3 positionMoins = new Vector3(transform.position.x * cos(Time.deltaTime * speed)
                                                + transform.position.z * sin(Time.deltaTime * speed),
                                            0,
                                            transform.position.z * cos(Time.deltaTime * speed)
                                                - transform.position.x * sin(Time.deltaTime * speed));
            if (Vector3.Distance(player.transform.position,
                                positionPlus)
                < Vector3.Distance(player.transform.position,
                                positionMoins))
            {
                agent.SetDestination(positionMoins);
            }
            else
            {
                agent.SetDestination(positionPlus);
            }
        }
        else
        {
            agent.SetDestination(-player.transform.position);
        }
    }

    private void ToItemMovement()
    {
        agent.SetDestination(itemTarget.transform.position);
    }

    private void RandomMovement()
    {
        if (Vector3.Distance(transform.position, arena.transform.position) > carrouselRadius || isReturningToSafeZone)
        {
            isReturningToSafeZone = Vector3.Distance(transform.position, arena.transform.position) > safeZoneRadius;
            randomPosition = Vector3.zero;
        }
        else if (timerRandomMovement < Mathf.Epsilon)
        {
            randomPosition = getRandomPosition(transform.position);
        }
        timerRandomMovement += Time.deltaTime;
        if (timerRandomMovement > randomTimeMovement)
        {
            timerRandomMovement = 0;
            randomTimeMovement = Random.Range(minRandomMovementTime, maxRandomMovementTime);
        }
        agent.SetDestination(randomPosition);
    }

    private Vector3 getRandomPosition(Vector3 janitorPosition)
    {
        Vector2 randomUnitCircle = Random.onUnitCircle;
        Vector3 randomPosition = new Vector3(randomUnitCircle.x, 0, randomUnitCircle.y);
        return randomPosition;
    }

    private float cos(float angle)
    {
        return Mathf.Cos(angle);
    }

    private float sin(float angle)
    {
        return Mathf.Sin(angle);
    }
}
