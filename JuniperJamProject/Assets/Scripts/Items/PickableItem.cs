using UnityEngine;

public abstract class PickableItem : Item
{ 
    [SerializeField] public Sprite sprite; //a serialise
    public void PickUp(ref PickableItem playerSlot, Transform playerTransform)
    {
        playerSlot = this;
        gameObject.SetActive(false);
        //transform.parent = playerTransform;
        transform.position = Vector3.zero;
        player = playerTransform.GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            player.SetItemToAssign(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerScript>(out PlayerScript player))
        {
            if(player.GetItemToAssign() == this)
            {
                player.SetItemToAssign(null);
            }
        }
    }
}
