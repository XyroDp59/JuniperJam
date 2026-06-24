using UnityEngine;

public class ThrowableItem : PickableItem
{
    [SerializeField] int NumberOfUses = 1;
    [SerializeField] Projectile projectile;
    [SerializeField] float distanceToPlayerAtSpawn = 0;
    [SerializeField] float heightAtSpawn = 0.5f;

    int currentUses = 0;

    private void Start()
    {
        OnItemDespawned.AddListener(ResetItem);
    }

    public override void Use()
    {
        if (currentUses >= NumberOfUses) return;
        Debug.Log($"Projectile {gameObject.name} : {NumberOfUses - currentUses} uses left");
        currentUses++;

        Projectile v = Instantiate(projectile);
        v.direction = player.GetMoveDirection().normalized;
        v.initialPos = player.transform.position + v.direction.normalized * distanceToPlayerAtSpawn;
        v.initialPos.y = heightAtSpawn;
        v.transform.position = v.initialPos;
        v.player = player;

        if (currentUses == NumberOfUses)
        {
            ItemSpawner.Singleton.DespawnItem(this);
            Debug.Log($"Projectile {gameObject.name} broke !");

            // Todo : UI
        }
    }

    void ResetItem()
    {
        currentUses = 0;
        transform.parent = ItemSpawner.Singleton.transform;
    }
}
