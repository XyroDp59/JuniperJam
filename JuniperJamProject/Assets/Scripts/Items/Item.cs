using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    [HideInInspector] public ItemFlickerData flicker;
    [HideInInspector] public PlayerScript player;
    [HideInInspector] public bool isItemActive;

    [SerializeField] MeshRenderer renderer;

    public UnityEvent OnItemSpawned = new UnityEvent();
    public UnityEvent OnItemDespawned = new UnityEvent();



    void Awake()
    {
        OnItemSpawned.AddListener(() => StartCoroutine(DespawnItem()));
    }

    IEnumerator DespawnItem()
    {
        yield return new WaitForSeconds(flicker.despawnCooldown - flicker.flickerDuration);

        float timer = 0f;
        while (timer < flicker.flickerDuration)
        {
            ChangeSpriteOpacity(flicker.flickerGradient.Evaluate(timer / flicker.flickerDuration));
            yield return null;
            timer += Time.deltaTime;
        }
        ItemSpawner.Singleton.DespawnItem(this);
    }

    void ChangeSpriteOpacity(float opacity)
    {
        Color w = Color.white; 
        renderer.material.SetColor("_BaseColor", new Color(w.r, w.g, w.b, opacity));
    }

    public abstract void Use();
}
