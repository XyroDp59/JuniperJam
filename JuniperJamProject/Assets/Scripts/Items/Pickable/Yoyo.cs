using System;
using UnityEngine;

public class Yoyo : PickableItem
{
    [SerializeField] int NumberOfUses;
    [SerializeField] YoyoProjectile projectile;
    [SerializeField] int damage;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float dashDistance;
    [SerializeField] private float approchingSpeed;

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
        
        YoyoProjectile y = Instantiate(projectile, player.transform.position, Quaternion.identity);
        y.player = player;
        y.damage = damage;
        y.dashSpeed = dashSpeed;
        y.speed = speed;
        y.dashDistance = dashDistance;
        y.dashDirection = new Vector3(player.GetMoveDirection().x, 0, player.GetMoveDirection().y);
        y.approchingSpeed = approchingSpeed;
        y.gameObject.SetActive(true);
        
        // SFX 
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Yoyo");
        
        
        
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
