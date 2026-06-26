using System.Collections;
using TMPro;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform damageableTransform;

    [SerializeField] float animDuration;
    [SerializeField] Gradient gradient;
    [SerializeField] AnimationCurve scale;
    [SerializeField] float thetaMax = 45;

    [SerializeField] float textDuration;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AnimationCurve textPosition;


    public void OnDamageTaken(int HpDiff, int currHp, float currPercentHp)
    {
        if (HpDiff >= 0) return;
        StartCoroutine(Animation());
        var newText = Instantiate(text, transform);
        newText.text = HpDiff.ToString();
    }

    IEnumerator Animation()
    {
        float yScale = damageableTransform.localScale.y;
        float xScale = damageableTransform.localScale.x;
        float zScale = damageableTransform.localScale.z;  

        float yRot = damageableTransform.localEulerAngles.y;

        var localPos = text.transform.localPosition;

        float time = 0;
        while (time < animDuration)
        {
            var color = gradient.Evaluate(time / animDuration);
            ChangeSpriteColor(color);

            float currScale = scale.Evaluate(time / animDuration);

            damageableTransform.localScale = (1 + currScale) * new Vector3(xScale, yScale, zScale);
            damageableTransform.localRotation = Quaternion.Euler(
                damageableTransform.localEulerAngles.x,
                yRot + currScale * thetaMax,           
                damageableTransform.localEulerAngles.z
            );
            text.transform.position = localPos + textPosition.Evaluate(time / animDuration) * Vector3.up;

            yield return null;
            time += Time.deltaTime;
        }

        ChangeSpriteColor(Color.white);
        damageableTransform.localScale = new Vector3(damageableTransform.localScale.x, yScale, damageableTransform.localScale.z);
        damageableTransform.localRotation = Quaternion.Euler(damageableTransform.localEulerAngles.x, yRot, damageableTransform.localEulerAngles.z);
    }

    public void ChangeSpriteColor(Color color)
    {
        meshRenderer.material.SetColor("_BaseColor", color);
    }
}
