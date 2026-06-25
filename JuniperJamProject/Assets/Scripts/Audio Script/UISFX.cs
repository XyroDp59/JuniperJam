using UnityEngine;

public class UISFX : MonoBehaviour
{
    public void HoverButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/hover");
    }
    public void BackButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/back");
    }
    
    public void ConfirmButtonSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/confirm");
    }
}

