using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    private FMOD.Studio.Bus _masterBus;
    private FMOD.Studio.Bus _soundtrackBus;
    private FMOD.Studio.Bus _sfxBus;

    private bool _isSFXClicked;

    void Awake()
    {
        _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        _soundtrackBus = FMODUnity.RuntimeManager.GetBus("bus:/Soundtrack");
        _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    }
    
    // ----------- OPTIONS ---------------------
    public void OnMasterVolumeChange(float value)
    {
        _masterBus.setVolume(value);
    }

    public void OnSoundtrackVolumeChange(float value)
    {
        _soundtrackBus.setVolume(value);
    }

    public void OnSFXVolumeChange(float value)
    {
        _sfxBus.setVolume(value);
        _isSFXClicked = true;
    }

    void Update()
    {
        // Preview for the SFX button
        if (Mouse.current.leftButton.wasReleasedThisFrame && _isSFXClicked)
        {
            _isSFXClicked = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/confirm");
        }
    }
}
