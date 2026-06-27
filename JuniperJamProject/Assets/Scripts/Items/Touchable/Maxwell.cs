using System.Collections;
using System.Runtime.CompilerServices;
using FMOD.Studio;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class Maxwell : MonoBehaviour
{
    [Header("Base Parameters")]
    [SerializeField] int attackPower = 100;
    [SerializeField] float speed = 10f;
    [SerializeField] string tag = "Wall";
    [SerializeField] float maxwellDuration = 5f;
    [SerializeField] float targetCircleSize = 3f;

    [Header("SFX")] 
    private FMOD.Studio.EventInstance maxwellInstance;

    Vector3 currentDir;
    [HideInInspector] public Rigidbody playerRb;
    [HideInInspector] public PlayerScript player;


    private void ActivateMaxwell(bool b)
    {
        player.GetMesh().gameObject.SetActive(!b);
        player.GetComponent<CapsuleCollider>().enabled = !b;
        player.TogglePlayerInput(!b);
        playerRb.useGravity = !b;


        // SFX
        Debug.Assert(SoundtrackController.Instance, "[Maxwell.cs] Soundtrack Controller must not be null");
        SoundtrackController.Instance.mainInstance.setPaused(b);
        if (b) SoundtrackController.Instance.maxwellInstance.start();
        else SoundtrackController.Instance.maxwellInstance.stop(STOP_MODE.IMMEDIATE);
    }

    private void Start()
    {
        ActivateMaxwell(true);
        currentDir = new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f,1.0f)).normalized;
        if (currentDir == Vector3.zero) currentDir = Vector3.left;

        StartCoroutine(MaxwellDeath());
        
        //SFX
        maxwellInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Sountracks/Maxwell");
    }

    IEnumerator MaxwellDeath()
    {
        yield return new WaitForSeconds(maxwellDuration);
        ActivateMaxwell(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            currentDir = ChooseNextDirection();
            Debug.Log($"Maxwell, go to {currentDir} !");
        }
        else if (other.gameObject != player.gameObject 
            && other.TryGetComponent<AttributSet>(out AttributSet health))
        {
            health.CurrentHp -= attackPower;
        }
    }

    private Vector3 ChooseNextDirection()
    {
        Vector2 posOnCircle = Random.onUnitCircle * targetCircleSize;
        Vector3 target = new Vector3(posOnCircle.x, 0, posOnCircle.y);
        return (target - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (playerRb == null)
        {
            Debug.LogError("shit");
        }
        Vector3 vel = currentDir * speed;
        vel.y = 0f;
        playerRb.linearVelocity = vel;
    }

}
