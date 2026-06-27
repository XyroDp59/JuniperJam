using UnityEngine;

public class HorseAnimation : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float amplitude = 1;
    float y;
    float phi;

    void Start()
    {
        y = transform.localPosition.y;
        phi = Random.value * Mathf.PI;
    }

    void Update()
    {
        float newY = y + amplitude * Mathf.Sin(phi + speed * Time.time);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
    }
}
