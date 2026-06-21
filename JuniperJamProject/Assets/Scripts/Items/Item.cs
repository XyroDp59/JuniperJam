using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : MonoBehaviour
{
    [SerializeField] float despawnCooldown;
    [SerializeField] float flickerDuration;
    [SerializeField] AnimationCurve flickerGradient;

    public UnityEvent OnItemSpawned = new UnityEvent();
    [HideInInspector] public PlayerScript player;

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
