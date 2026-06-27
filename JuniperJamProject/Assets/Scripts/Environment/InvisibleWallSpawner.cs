using UnityEngine;

public class InvisibleWallSpawner : MonoBehaviour
{
    [SerializeField] GameObject WallPrefab;
    [SerializeField] int deltaRadius = 15;

    void Awake()
    {
        for(int i = 0; i < 360; i += deltaRadius)
        {
            var wall = Instantiate(WallPrefab, transform);
            wall.transform.localRotation = Quaternion.Euler(new Vector3(0, i, 0));
        }
    }
}
