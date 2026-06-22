using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent (typeof(SphereCollider))]
public class Vortex : MonoBehaviour
{
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Vector3 initialPos;

    [SerializeField] private float speed = 2;
    [SerializeField] AnimationCurve vortexSizeCurve;
    [SerializeField] float defaultHeight = 1;
    [SerializeField] float curveSize = 3;
    [SerializeField] float vortexStrength = 1;
    [SerializeField] float vortexDuration = 3;
    [SerializeField] float vortexSize = 2;

    SphereCollider col;

    void Start()
    {
        col = GetComponent<SphereCollider>();
        StartCoroutine(Trajectory());
        Destroy(gameObject, vortexDuration * 1.2f);
    }

    IEnumerator Trajectory()
    {
        // Point de départ réel sur la courbe (t = vortexDuration = spirale la plus large)
        Vector3 startOnCurve = CalculateGoldenRatioPos(vortexDuration);

        // Offset pour que ce point coïncide exactement avec initialPos
        Vector3 offset = initialPos - startOnCurve;

        float timer = 0;
        while (timer < vortexDuration)
        {
            float t = vortexDuration - timer; // part grand, descend vers 0
            transform.position = CalculateGoldenRatioPos(t) + offset;
            col.radius = vortexSizeCurve.Evaluate(timer / vortexDuration) * vortexSize;
            timer += Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(-vortexStrength * (transform.position - other.transform.position));
        }           
    }

    private Vector3 CalculateGoldenRatioPos(float t)
    {
        Vector3 playerForward = new Vector3(direction.x, 0f, direction.y).normalized;

        if (playerForward == Vector3.zero)
            playerForward = Vector3.forward;

        Vector3 playerRight = Vector3.Cross(Vector3.up, playerForward).normalized;

        float phi = 1.618f;
        float sizeFactor = curveSize * Mathf.Pow(phi, 2 * t / Mathf.PI);

        return initialPos
            + sizeFactor * Mathf.Cos(t) * playerRight
            + defaultHeight * Vector3.up
            + sizeFactor * Mathf.Sin(t) * playerForward;
    }
}
