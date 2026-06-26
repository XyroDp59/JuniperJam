using System.Collections;
using TMPro;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [Header("targets")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Transform damageableTransform;

    [Header("damage animation")]
    [SerializeField] float animDuration;
    [SerializeField] Gradient gradient;
    [SerializeField] AnimationCurve scale;
    [SerializeField] float thetaMax = 45;

    [Header("Text damage")]
    [SerializeField] float textDuration;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AnimationCurve textPosition;

    [Header("death animation")]
    [SerializeField] float deathAnimDuration;
    [SerializeField] Gradient deathGradient;
    [SerializeField] AnimationCurve deathAngularVelocity;
    [SerializeField] float maxDeathAngularVelocity = 50;
    [SerializeField] GameObject deathParticles;

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

    public void OnDeath()
    {
        StartCoroutine(DeathAnimation());
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

    IEnumerator DeathAnimation()
    {
        float time = 0;
        while (time < deathAnimDuration)
        {
            // color
            var color = deathGradient.Evaluate(time / deathAnimDuration);
            ChangeSpriteColor(color);

            // squish
            float angularSpeed = deathAngularVelocity.Evaluate(time / deathAnimDuration);
            damageableTransform.Rotate(0f, angularSpeed * maxDeathAngularVelocity * Time.deltaTime, 0f, Space.Self);

            yield return null;
            time += Time.deltaTime;
        }
        var temp = Instantiate(deathParticles, damageableTransform.position, Quaternion.identity);
        damageableTransform.gameObject.SetActive(false);
        Destroy(temp.gameObject, 2f);
    }
}
