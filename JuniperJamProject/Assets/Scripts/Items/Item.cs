using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            player.itemToAssign = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            if(player.itemToAssign == this)
            {
                player.itemToAssign = null;
            }
        }
    }


    public void PickUp(ref Weapon playerSlot, Transform playerTransform)
    {
        playerSlot = weapon;
        weapon.gameObject.SetActive(false);
        weapon.transform.parent = playerTransform;
        weapon.transform.position = Vector3.zero;
        Destroy(gameObject);
    }
}
