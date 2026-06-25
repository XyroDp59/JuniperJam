using UnityEngine;

public class Heart : TouchableItem
{
    [SerializeField] int HealingPower = 1;
    public override void Use()
    {
        player.GetComponent<AttributSet>().CurrentHp += 1;
        Debug.Log("heart used");
        
        //SFX
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Heart");
    }
}
