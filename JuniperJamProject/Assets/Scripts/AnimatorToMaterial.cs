using System;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class AnimatorToMaterial : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material material;
    [SerializeField] float widthImage;
    [SerializeField] float heightImage;
    [SerializeField] float widthSprite;
    [SerializeField] float heightSprite;
    [SerializeField] float xBaseOffset;
    [SerializeField] float yBaseOffset;

    private void Awake()
    {
        material.mainTextureScale = new Vector2(widthSprite / widthImage, heightSprite / heightImage);
    }

    void Update()
    {
        //print(spriteRenderer.sprite.textureRectOffset);
        //print(spriteRenderer.sprite.spriteAtlasTextureScale);
        //material.SetTextureScale("_MainTex", new Vector2(1f, 1f));
        material.mainTextureOffset = new Vector2((xBaseOffset + spriteRenderer.sprite.rect.x) / widthImage, (yBaseOffset + spriteRenderer.sprite.rect.y) / heightImage);
        //material.SetTexture(0, spriteRenderer.sprite.texture);
    }
}
