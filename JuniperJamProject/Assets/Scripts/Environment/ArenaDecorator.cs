using UnityEngine;

public class ArenaDecorator : MonoBehaviour
{
    [SerializeField] Transform plateformTransform;
    [SerializeField] GameObject polePrefab;
    [SerializeField] int deltaRadius = 15;

    void Awake()
    {
        for (int i = 0; i < 360; i += deltaRadius)
        {
            var pole = Instantiate(polePrefab, plateformTransform);
            pole.transform.localRotation = Quaternion.Euler(new Vector3(0, i, 0));
        }
    }
}
