using TMPro;
using UnityEngine;

public class DamageVisualizer : MonoBehaviour
{
    [SerializeField] Gradient color;
    [SerializeField] AnimationCurve colorPicker;
    [SerializeField] AnimationCurve rotation;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public void OnDamageTaken()
    {

    }
}
