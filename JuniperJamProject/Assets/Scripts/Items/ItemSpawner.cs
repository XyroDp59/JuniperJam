using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] List<Item> itemList = new List<Item>();
    [SerializeField] int poolSize;

    [Header("Spawn Animation")]
    [SerializeField] float arenaSize = 12f;
    [SerializeField] AnimationCurve throwCurve;
    [SerializeField] float throwDuration = 1;
    [SerializeField] float cooldownInBetweenThrow = 5f;
    [SerializeField] float throwCooldownDurationVariation = 1.5f;
    [SerializeField] float throwHeight;
    Coroutine spawnCoroutine;
    [SerializeField] ItemFlickerData flickerData;

    List<Item> itemPool = new List<Item>();
    List<Item> activeItems = new List<Item>();

    public static ItemSpawner Singleton;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;

            for(int i = 0; i< poolSize; i++)
            {
                Item item = Instantiate(itemList[Random.Range(0, itemList.Count)], transform);

                item.flicker = flickerData;
                item.gameObject.SetActive(false);
                itemPool.Add(item);
            }
        }
    }

    private void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnItemFromCrowd());   
    }

    private IEnumerator SpawnItemFromCrowd()
    {
        yield return new WaitForSeconds(cooldownInBetweenThrow + Random.Range(0, throwCooldownDurationVariation));

        if (itemPool.Count != 0)
        {
            Vector2 _spawnPoint = Random.onUnitCircle * 1.5f * arenaSize;
            Vector3 realSpawnPoint = new Vector3(_spawnPoint.x, 0, _spawnPoint.y);
            var item = _SpawnItem(realSpawnPoint);

            Vector2 _target = Random.insideUnitCircle * 0.9f * arenaSize;
            Vector3 realTarget = new Vector3(_target.x, 0, _target.y);

            float timer = 0;
            while (timer < throwDuration)
            {
                float height = throwHeight * throwCurve.Evaluate(timer / throwDuration);
                Vector3 newPos = Vector3.Lerp(realSpawnPoint, realTarget, timer / throwDuration);
                newPos.y = height;
                item.transform.position = newPos;

                yield return null;
                timer += Time.deltaTime;
            }
        }

        spawnCoroutine = StartCoroutine(SpawnItemFromCrowd());
    }

    public Item SpawnItem(Vector3 position)
    {
        if (itemPool.Count == 0) return null;

        StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnItemFromCrowd());

        return _SpawnItem(position);
    }


    private Item _SpawnItem(Vector3 position)
    {
        if (itemPool.Count == 0) return null;

        Item item = itemPool[Random.Range(0, itemPool.Count)];

        item.gameObject.SetActive(true);

        itemPool.Remove(item);
        activeItems.Add(item);
        item.isItemActive = true;
        item.OnItemSpawned.Invoke();
        return item;
    }

    public Item SpawnItemWithoutAnim(Item itemToSpawn, Vector3 position)
    {
        for (int i = 0;  i < itemPool.Count; i++)
        {
            if (itemPool[i].name.Equals(itemToSpawn.name))
            {
                Item item = itemPool[i];
                item.gameObject.SetActive(true);

                itemPool.Remove(item);
                activeItems.Add(item);
                item.isItemActive = true;
                item.OnItemSpawned.Invoke();
                Instantiate(item, position, transform.rotation);
                return item;
            }
        }
        return null;
    }

    public void DespawnItem(Item item)
    {
        item.gameObject.SetActive(false);
        item.isItemActive = false;
        item.transform.parent = transform.parent;
        Debug.Assert(activeItems.Contains(item), "An item spawned manually got into the pool !");

        activeItems.Remove(item);
        itemPool.Add(item);
        item.OnItemDespawned.Invoke();
    }

    public List<Item> getActiveItems()
    {
        return activeItems;
    }
}
