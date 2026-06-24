using System.Collections;
using UnityEngine;

public class MaxwellItem : TouchableItem
{
    [SerializeField] Maxwell maxwellPrefab;
    Maxwell maxwell;

    public override void Use()
    {
        maxwell = Instantiate(maxwellPrefab, player.transform);
        maxwell.playerRb = player.GetComponent<Rigidbody>();
        maxwell.player = player;
    }
}
