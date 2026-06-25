using System.Collections;
using TMPro;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform transform;

    [SerializeField] float animDuration;
    [SerializeField] Gradient gradient;
    [SerializeField] AnimationCurve scale;
    [SerializeField] AnimationCurve rotation;

    [SerializeField] float textDuration;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] AnimationCurve textPosition;


    public void OnDamageTaken()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float yScale = transform.localScale.y;
        float yRot = transform.localEulerAngles.y;

        float time = 0;
        while (time < animDuration)
        {
            var color = gradient.Evaluate(time / animDuration);
            transform.localScale = new Vector3(transform.localScale.x, scale.Evaluate(time/animDuration), transform.localScale.z);
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, rotation.Evaluate(time/animDuration), transform.localRotation.z);
            yield return null;
            time += Time.deltaTime;
        }

        ChangeSpriteColor(Color.white);
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, yRot, transform.localRotation.z);
    }

    public void ChangeSpriteColor(Color color)
    {
        meshRenderer.material.SetColor("_Color", color);
    }
}
