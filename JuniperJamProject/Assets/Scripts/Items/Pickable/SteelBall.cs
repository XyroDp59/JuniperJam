using UnityEngine;

public class SteelBall : PickableItem
{
    [SerializeField] int NumberOfUses = 1;
    [SerializeField] Vortex projectile;
    [SerializeField] int damage;

    int currentUses = 0;

    private void Start()
    {
        OnItemDespawned.AddListener(ResetItem);
    }

    public override void Use()
    {
        if (currentUses >= NumberOfUses) return;
        Debug.Log($"Steel Ball : {NumberOfUses - currentUses}");
        currentUses++;

        Vortex v = Instantiate(projectile);
        v.initialPos = player.transform.position;
        v.direction = player.GetMoveDirection();

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
}
