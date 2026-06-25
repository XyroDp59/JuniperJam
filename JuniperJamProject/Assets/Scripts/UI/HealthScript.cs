using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    
    [SerializeField] private AttributSet target;
    
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text healthText;
    
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private bool colorizedText = false;
    [SerializeField] private float smoothness = 6f;

    private float targetFill = 1f;

    private void OnEnable()
    {
        if (target == null)
        {
            return;
        }
        target.onHpChange.AddListener(OnHpChange);
        OnHpChange(target.CurrentHp, (float) target.CurrentHp / target.MaxHp);
    }

    private void OnDisable()
    {
        if (target != null)
            target.onHpChange.RemoveListener(OnHpChange);
    }

    private void OnHpChange(int currHp, float ratio)
    {
        targetFill = ratio;
        if (healthText != null) healthText.text = currHp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (fillImage == null) return;
        fillImage.fillAmount = Mathf.MoveTowards(fillImage.fillAmount, targetFill, smoothness * Time.deltaTime);
        if (colorGradient != null)
        {
            Color c = colorGradient.Evaluate(fillImage.fillAmount);
            fillImage.color = c;
            if (colorizedText && healthText != null) healthText.color = c;
        }

        /*
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            target.CurrentHp -= 5;
        }*/
    }
}
