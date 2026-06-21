using System;
using UnityEngine;

public class Yoyo : PickableItem
{
    [SerializeField] int NumberOfUses;
    [SerializeField] GameObject projectile;
    [SerializeField] int damage;

    public override void Use()
    {
        if (NumberOfUses <= 0) return;
        NumberOfUses--;
        Debug.Log($"Yoyo : {NumberOfUses}");
        if (NumberOfUses == 0)
        {
            Destroy(gameObject);
            // Todo : UI
        }
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
