using UnityEngine;

public class SoundtrackController : MonoBehaviour
{
    public static SoundtrackController Instance;
    public FMOD.Studio.EventInstance mainInstance;
    public FMOD.Studio.EventInstance maxwellInstance;
    public FMOD.Studio.EventInstance endInstance;

    void Awake()
    {
        Instance = this;
        mainInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/Main game");
        maxwellInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/Maxwell");
        endInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/End");
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
}
