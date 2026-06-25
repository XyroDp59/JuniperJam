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
            player.itemToAssign = this;
            // Todo : shader + UI
            
            
            //SFX
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Pickup");
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
            // Todo : shader + UI
        }
    }
}
