using UnityEngine;

[CreateAssetMenu(fileName = "ItemFlickerData", menuName = "Scriptable Objects/ItemFlickerData")]
public class ItemFlickerData : ScriptableObject
{
    [SerializeField, Range(1f, 60f)] 
    public float despawnCooldown = 5f;

    [SerializeField] 
    public float flickerDuration = 1f;
    
    [SerializeField]
    public AnimationCurve flickerGradient;
}
