using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.TryGetComponent(out AttributSet health))
        {
            health.CurrentHp -= int.MaxValue;
        }
    }
}
