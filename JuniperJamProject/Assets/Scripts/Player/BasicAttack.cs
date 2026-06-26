using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BasicAttack : MonoBehaviour
{
    public int damage;
    public float timeToLive = 0.25f;
    public float currentLifeTime;

    private void OnEnable()
    {
        currentLifeTime = 0;
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= timeToLive) gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out AttributSet attributSet))
        {
            attributSet.CurrentHp -= damage;
            
            // SFX
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Smash");
        }
    }
}
