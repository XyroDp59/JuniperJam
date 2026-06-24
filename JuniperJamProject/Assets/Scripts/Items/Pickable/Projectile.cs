using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPos;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public PlayerScript player;
}
