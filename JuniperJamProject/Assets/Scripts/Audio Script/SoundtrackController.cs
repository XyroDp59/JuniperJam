using FMOD.Studio;
using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    public static SoundtrackController Instance;
    public FMOD.Studio.EventInstance mainInstance;
    public FMOD.Studio.EventInstance maxwellInstance;
    public FMOD.Studio.EventInstance endInstance;
    
    private FMOD.Studio.Bus masterBus;

    void Awake()
    {
        Instance = this;
        mainInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/Main game");
        maxwellInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/Maxwell");
        endInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/End");

        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainInstance.start();
    }

    void OnDestroy()
    {
        mainInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }


    public void OnPlayerDeath()
    {
        masterBus.stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Pain");
        
    }
}
