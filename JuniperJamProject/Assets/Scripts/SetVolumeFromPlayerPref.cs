using UnityEngine;

public class SetVolumeFromPlayerPref : MonoBehaviour
{
    private FMOD.Studio.Bus _masterBus;
    private FMOD.Studio.Bus _soundtrackBus;
    private FMOD.Studio.Bus _sfxBus;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        _soundtrackBus = FMODUnity.RuntimeManager.GetBus("bus:/Soundtrack");
        _sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        
        _masterBus.setVolume(PlayerPrefs.GetFloat("MasterVol",1f));
        _soundtrackBus.setVolume(PlayerPrefs.GetFloat("MusicVol",1f));
        _sfxBus.setVolume(PlayerPrefs.GetFloat("SFXVol",1f));
    }
}
