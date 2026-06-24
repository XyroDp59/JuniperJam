using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RotatingArena : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Rigidbody rb;
    private float currentRotation;

    public static RotatingArena Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation += rotateSpeed * Time.deltaTime;
        rb.MoveRotation(Quaternion.Euler(0, currentRotation, 0));
    }
}
