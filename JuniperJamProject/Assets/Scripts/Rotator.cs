using UnityEngine;

public class Rotator : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [SerializeField] Transform target;
    [SerializeField] float angularSpeed;
    [SerializeField] Axis axis;

    void Update()
    {
        Vector3 dir = axis switch
        {
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            _ => Vector3.up
        };

        target.Rotate(dir, angularSpeed * Time.deltaTime);
    }
}