using UnityEngine;

public class TransparenceScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MeshRenderer meshRenderer;
    
    private GameObject player = null;
    private Material saveMaterial;

    private void Awake()
    {
        saveMaterial = spriteRenderer.material;
    }

    void Update()
    {
        if (player != null && player.activeSelf)
        {
            //tentative de raciste qui marche pas
            //if (Mathf.Abs(transform.position.z - player.transform.position.z) < 5
            //    && (transform.position.z - player.transform.position.z) < 0
            //    && Mathf.Abs(transform.position.x - player.transform.position.x) < 1)
            //{
            //    spriteRenderer.material = meshRenderer.material;
            //    spriteRenderer.material.SetColor("_BaseColor", new Color(spriteRenderer.material.color.r,
            //                                            spriteRenderer.material.color.g,
            //                                            spriteRenderer.material.color.b, 0));
            //}
            //else
            //{
            //    spriteRenderer.material = saveMaterial;
            //}
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            player = other.gameObject;
        }
    }
}
