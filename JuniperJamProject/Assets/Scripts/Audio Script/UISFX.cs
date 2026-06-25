using UnityEngine;

public class UISFX : MonoBehaviour
{
    public void HoverButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/hover");
    }
    public void BackButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/back");
    }
    
    public void ConfirmButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/confirm");
    }
}

