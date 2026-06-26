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

    Vector3 localScale;
    Vector3 localRot;

    private void Start()
    {
        localScale = damageableTransform.localScale;
        localRot = damageableTransform.localRotation.eulerAngles;
    }

    public void OnDamageTaken(int HpDiff, int currHp, float currPercentHp)
    {
        if (HpDiff >= 0) return;
        StartCoroutine(Animation(HpDiff));
    }

    IEnumerator Animation(int HpDiff)
    {
        var newText = Instantiate(text, transform);
        newText.text = $"{HpDiff} HP";
        newText.rectTransform.localPosition = Vector3.zero;

        float time = 0;
        while (time < animDuration)
        {
            // color
            var color = gradient.Evaluate(time / animDuration);
            ChangeSpriteColor(color);

            // squish
            float currScale = scale.Evaluate(time / animDuration);
            damageableTransform.localScale = (1 + currScale) * localScale;
            damageableTransform.localRotation = Quaternion.Euler( localRot.x, localRot.y + currScale * thetaMax, localRot.z);

            // texte
            newText.rectTransform.localPosition = textPosition.Evaluate(time / animDuration) * Vector3.up;

            yield return null;
            time += Time.deltaTime;
        }

        ChangeSpriteColor(Color.white);
        damageableTransform.localScale = localScale;
        damageableTransform.localRotation = Quaternion.Euler(localRot);
        Destroy(newText.gameObject);
    }

    public void ChangeSpriteColor(Color color)
    {
        meshRenderer.material.SetColor("_BaseColor", color);
    }
}
