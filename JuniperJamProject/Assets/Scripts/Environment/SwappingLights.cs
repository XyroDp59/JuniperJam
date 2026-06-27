using UnityEngine;

public class SwappingLights : MonoBehaviour
{
    [SerializeField] Gradient gradient;
    [SerializeField] float speed = 1f;
    public float offset;

    Light light;
    float t;
    bool reversed;

    void Start()
    {
        light = GetComponent<Light>();
        t = offset;
    }

    void Update()
    {
        t += (reversed ? -1f : 1f) * speed * Time.deltaTime;

        if (t >= 1f) { t = 1f; reversed = true; }
        else if (t <= 0f) { t = 0f; reversed = false; }

        light.color = gradient.Evaluate(t);
    }
}