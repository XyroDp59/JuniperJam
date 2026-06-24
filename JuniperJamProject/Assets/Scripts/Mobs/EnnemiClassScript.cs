using System;
using UnityEngine;

[RequireComponent(typeof(AttributSet))]
public class EnnemiClassScript : MonoBehaviour
{
    public int rewardScore;
    public AttributSet attributSet; //serialized for better performance
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
