using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public PlayerScript player;
    abstract public void Use();
}
