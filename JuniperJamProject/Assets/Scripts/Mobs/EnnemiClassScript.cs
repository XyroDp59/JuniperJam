using NUnit.Framework;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(AttributSet))]
public class EnnemiClassScript : MonoBehaviour
{
    public int rewardScore;
    public AttributSet attributSet; //serialized for better performance
    [SerializeField] private int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out AttributSet attributSet) && collision.gameObject.layer != 6)
        {
            attributSet.CurrentHp -= damage;
        }
    }
    public float slownessFactor = 1;

    [SerializeField] private int attackPower = 5;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerScript>(out var player) && other.TryGetComponent<AttributSet>(out var health))
        {
            health.CurrentHp -= attackPower;
        }
    }
}
