using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    [SerializeField, Range(1f, 60f)] float despawnCooldown = 5f;
    [SerializeField] float flickerDuration;
    [SerializeField] AnimationCurve flickerGradient;

    public UnityEvent OnItemSpawned = new UnityEvent();
    public UnityEvent OnItemDespawned = new UnityEvent();

    [HideInInspector] public PlayerScript player;

    public bool isItemActive;


    void Awake()
    {
        OnItemSpawned.AddListener(() => StartCoroutine(DespawnItem()));
    }

    IEnumerator DespawnItem()
    {
        yield return new WaitForSeconds(despawnCooldown);

        float timer = 0f;
        while (timer < flickerDuration)
        {
            ChangeSpriteOpacity(flickerGradient.Evaluate(timer / flickerDuration));
            yield return null;
        }
        ItemSpawner.Singleton.DespawnItem(this);
    }

    void ChangeSpriteOpacity(float opacity)
    {
        // TODO
    }

    public abstract void Use();
}
