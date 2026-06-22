using System;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AnimatorToMaterial : MonoBehaviour
{
    private static readonly int DoubleSidedEnable = Shader.PropertyToID("_DoubleSidedEnable");
    private static readonly int AlphaCutoff = Shader.PropertyToID("_AlphaCutoff");
    private static readonly int AlphaCutoffEnable = Shader.PropertyToID("_AlphaCutoffEnable");
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SpriteRenderer spriteRenderer;
    Material _material;
    [SerializeField] float widthImage;
    [SerializeField] float heightImage;
    [SerializeField] float widthSprite;
    [SerializeField] float heightSprite;
    [SerializeField] float xBaseOffset;
    [SerializeField] float yBaseOffset;

    private void Awake()
    {
        _material = new Material(Shader.Find("HDRP/Lit"));
        print(DoubleSidedEnable);
        _material.SetFloat(DoubleSidedEnable, 1f);
        _material.SetFloat(AlphaCutoffEnable, 1f);
        _material.SetFloat(AlphaCutoff, 0.5f);
        _material.mainTexture = spriteRenderer.sprite.texture;
        _material.mainTextureScale = new Vector2(widthSprite / widthImage, heightSprite / heightImage);
        HDMaterial.ValidateMaterial(_material);
        meshRenderer.material = _material;
    }

    void Update()
    {
        _material.mainTextureOffset = new Vector2((xBaseOffset + spriteRenderer.sprite.rect.x) / widthImage, (yBaseOffset + spriteRenderer.sprite.rect.y) / heightImage);
    }
}
