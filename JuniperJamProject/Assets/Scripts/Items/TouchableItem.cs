using UnityEngine;

public abstract class TouchableItem : Item
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerScript>(out PlayerScript otherPlayer))
        {
            player = otherPlayer;
            Use();
            ItemSpawner.Singleton.DespawnItem(this);
        }
    }
}
