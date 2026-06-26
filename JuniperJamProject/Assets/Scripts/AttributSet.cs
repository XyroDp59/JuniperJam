using System;
using UnityEngine;
using UnityEngine.Events;

public class AttributSet : MonoBehaviour
{
    [SerializeField] private int maxHp;
    private int currentHp;
    public bool invulnerable;
    
    
    // Kaily thingy (Apparently, onDeath were Invoke a second time after, and I don't want that for the SFX)
    private bool dead;
    
    public int CurrentHp
    {
        get {  return currentHp; }
        set
        {
            if (invulnerable && value < currentHp) {print("nope"); return;}
            value = Mathf.Clamp(value, 0, maxHp);
            int previousHp = currentHp;
            currentHp = value;
            onHpChange.Invoke(value - previousHp, currentHp, (float)currentHp / maxHp);
            if (currentHp <= 0 & !dead)
            {
                onDeath.Invoke();
                dead = true;
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
