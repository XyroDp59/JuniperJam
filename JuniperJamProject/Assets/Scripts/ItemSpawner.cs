using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] List<Item> itemList = new List<Item>();
    [SerializeField] Item itemPrefab;
    [SerializeField] int poolSize;
    [SerializeField] AnimationCurve curve;

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
                Item item = Instantiate(itemList[Random.Range(0, itemList.Count)]);

                item.gameObject.SetActive(false);
                itemPool.Add(item);
            }
        }
    }

    public void SpawnItemFromCrowd()
    {
        // Todo : handle to cancel this coroutine when an item is spawned (manually or not)
    }

    public void SpawnItem(Vector3 position)
    {
        Item item = itemPool[Random.Range(0, itemPool.Count)];

        item.gameObject.SetActive(true);

        itemPool.Remove(item);
        activeItems.Add(item);
    }

    public void DespawnItem(Item item)
    {
        item.gameObject.SetActive(false);
        Debug.Assert(activeItems.Contains(item));

        activeItems.Remove(item);
        itemPool.Add(item);
    }
}
