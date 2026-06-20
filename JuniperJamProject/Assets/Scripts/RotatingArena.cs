using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatingArena : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Rigidbody rb;
    private float currentRotation;

    // Update is called once per frame
    void Update()
    {
        currentRotation += rotateSpeed * Time.deltaTime;
        rb.MoveRotation(Quaternion.Euler(0, currentRotation, 0));
    }
}
