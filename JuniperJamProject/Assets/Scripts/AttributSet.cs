using System;
using UnityEngine;
using UnityEngine.Events;

public class AttributSet : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;
    public int CurrentHp
    {
        get {  return currentHp; }
        set
        {
            value = Mathf.Clamp(value, 0, maxHp);
            int previousHp = currentHp;
            currentHp = value;
            onHpChange.Invoke(value - previousHp, currentHp, (float)currentHp / maxHp);
            if (currentHp <= 0)
            {
                onDeath.Invoke();
            }
            Debug.Log($"Health : {value}");
        }
    }
    public int MaxHp => maxHp;

    public UnityEvent<int, int, float> onHpChange; // <damageTaken, currentHp, currentPourcentage> (post change) 
    public UnityEvent onDeath;

    private void Awake()
    {
        CurrentHp = maxHp;
    }
}
