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
        
        _masterBus.setVolume(PlayerPrefs.GetFloat("MasterVol",1f));
        _soundtrackBus.setVolume(PlayerPrefs.GetFloat("MusicVol",1f));
        _sfxBus.setVolume(PlayerPrefs.GetFloat("SFXVol",1f));
    }
    
    
    // ----------- OPTIONS ---------------------
    public void OnMasterVolumeChange(float value)
    {
        _masterBus.setVolume(value);
        PlayerPrefs.SetFloat("MasterVol", value);
    }

    public void OnSoundtrackVolumeChange(float value)
    {
        _soundtrackBus.setVolume(value);
        PlayerPrefs.SetFloat("MusicVol", value);
    }

    public void OnSFXVolumeChange(float value)
    {
        _sfxBus.setVolume(value);
        PlayerPrefs.SetFloat("SFXVol", value);
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
