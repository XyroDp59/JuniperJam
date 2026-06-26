using System;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public void OnDamageTaken(Int32 damage, Int32 b, Single c)
    {
        if (damage < 0)  FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Pain");
    }
}
