using System;
using UnityEngine;

public class Yoyo : PickableItem
{
    [SerializeField] int NumberOfUses;
    [SerializeField] GameObject projectile;
    [SerializeField] int damage;

    int currentUses = 0;

    private void Start()
    {
        OnItemDespawned.AddListener(ResetItem);
    }

    public override void Use()
    {
        if (currentUses >= NumberOfUses) return;
        Debug.Log($"Yoyo : {NumberOfUses - currentUses}");
        currentUses++;
        if (currentUses == NumberOfUses)
        {
            ItemSpawner.Singleton.DespawnItem(this);
            Debug.Log($"Yoyo broke !");
            
            // Todo : UI
        }
    }

    void ResetItem()
    {
        currentUses = 0;
        transform.parent = ItemSpawner.Singleton.transform;
    }

    /*

    private void OnTriggerEnter(Collider other)
    {
        if(other != player && other.TryGetComponent<AttributSet>(out AttributSet health))
        {
            health.CurrentHp -= damage;
        }
    }*/
}
