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
        set { 
            value = Mathf.Clamp(value, 0, maxHp);
            onHpChange.Invoke(value - currentHp, value, ((float)value) / maxHp); 
            if (value <= 0) onDeath.Invoke();
            
            currentHp = value;
            Debug.Log("player hp :" + currentHp);
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
