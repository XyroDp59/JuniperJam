using System.Collections;
using UnityEngine;

public class Platine : PickableItem
{
    [SerializeField] private PlatineDisk platineDisk;
    [SerializeField] private float platineTime;
    public override void Use()
    {
        PlatineDisk disk = Instantiate(platineDisk, player.transform);
        disk.player = player;
        disk.platineTime = platineTime;
        disk.gameObject.SetActive(true);
        ItemSpawner.Singleton.DespawnItem(this);
    }
}
