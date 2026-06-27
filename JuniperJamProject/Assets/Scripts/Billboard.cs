using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Vector3 orientation;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(orientation);
    }
}
