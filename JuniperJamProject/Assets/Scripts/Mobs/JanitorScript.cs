using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JanitorScript : MonoBehaviour
{
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
    private Vector3 randomDirection = Vector3.zero;

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
        randomDirection = Vector3.zero;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        items = itemSpawner.getActiveItems();
        Debug.Log(items.Count);
        isItemOnArena = items.Count != 0;
        if (items.Count == 1 && itemTarget != items[0])
        {
            timerForItem = 0;
            itemTarget = items[0];
        }
        if (isItemOnArena && timerForItem > timeToWaitBeforeChassingItem)
        {
            Debug.Log(distanceToPlayer);
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
        //if (Vector3.Distance(transform.position, arena.transform.position) >= carrouselRadius)
        //{
        //    float angle = calculAngle(transform.position);
        //    Vector3 positionPlus = new Vector3(cos(angle + Time.deltaTime * speed),
        //                                    0,
        //                                    sin(angle + Time.deltaTime * speed));
        //    Vector3 positionMoins = new Vector3(cos(angle - Time.deltaTime * speed),
        //                        0,
        //                        sin(angle - Time.deltaTime * speed));
        //    if (Vector3.Distance(player.transform.position,
        //                        positionPlus)
        //        < Vector3.Distance(player.transform.position,
        //                        positionMoins))
        //    {
        //        transform.position = positionMoins;
        //    }
        //    else
        //    {
        //        transform.position = positionPlus;
        //    }
        //}
        //else
        //{
            Vector3 fleeDirection = getFleeDirection(transform.position, player.transform.position);
            transform.position += fleeDirection * Time.deltaTime * speed;
        //}
    }

    private void ToItemMovement()
    {
        Vector3 toItemDirection = getDirection(transform.position, itemTarget.transform.position);
        transform.position += toItemDirection * Time.deltaTime * speed;
    }

    private void RandomMovement()
    {
        if (Vector3.Distance(transform.position, arena.transform.position) > carrouselRadius || isReturningToSafeZone)
        {
            isReturningToSafeZone = Vector3.Distance(transform.position, arena.transform.position) > safeZoneRadius;
            randomDirection = getDirection(transform.position, Vector3.zero);
        }
        else if (timerRandomMovement < Mathf.Epsilon)
        {
            randomDirection = getRandomDirection(transform.position);
        }
        timerRandomMovement += Time.deltaTime;
        if (timerRandomMovement > randomTimeMovement)
        {
            timerRandomMovement = 0;
            randomTimeMovement = Random.Range(minRandomMovementTime, maxRandomMovementTime);
        }
        transform.position += randomDirection * Time.deltaTime * speed;
    }
    private Vector3 getDirection(Vector3 janitorPosition, Vector3 position)
    {
        Vector3 direction = position - janitorPosition;
        direction.y = 0;                                         //janitor cannot jump
        return Vector3.Normalize(direction);
    }

    private Vector3 getFleeDirection(Vector3 janitorPosition, Vector3 playerPosition)
    {
        Vector3 fleeDirection = - getDirection(janitorPosition, playerPosition);
        return fleeDirection;
    }

    private Vector3 getRandomDirection(Vector3 janitorPosition)
    {
        Vector2 randomUnitCircle = Random.onUnitCircle;
        Vector3 randomPosition = new Vector3(randomUnitCircle.x, 0, randomUnitCircle.y);
        Vector3 randomDirection = getDirection(janitorPosition, randomPosition);
        return randomDirection;
    }

    private float calculAngle(Vector3 janitorPosition)
    {
        int signeAngle = 1;
        if (janitorPosition.z < 0)
        {
            signeAngle = -1;
        }
        float angle = Mathf.Acos(Mathf.Abs(janitorPosition.x) / carrouselRadius) * signeAngle;
        Debug.Log(Mathf.Acos(Mathf.Abs(janitorPosition.x) / carrouselRadius));
        return angle;
    }
    private float cos(float angle)
    {
        return Mathf.Cos(angle) * carrouselRadius;
    }

    private float sin(float angle)
    {
        return Mathf.Sin(angle) * carrouselRadius;
    }
}
