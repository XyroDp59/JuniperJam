using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AnimatorToMaterial : MonoBehaviour
{
    /* // URP/Lit property IDs
        private static readonly int Cull = Shader.PropertyToID("_Cull");
        private static readonly int AlphaClip = Shader.PropertyToID("_AlphaClip");
        private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");
        private static readonly int Surface = Shader.PropertyToID("_Surface");
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");*/

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float widthImage;
    [SerializeField] float heightImage;
    [SerializeField] float widthSprite;
    [SerializeField] float heightSprite;
    [SerializeField] float xBaseOffset;
    [SerializeField] float yBaseOffset;

    private float _spinRemainingDuration = 0;
    private float _spinTotalDuration = 0;
    private int _numberOfSpin = 1;

    [SerializeField] Material referenceMaterial;
    Material _material;

    private void Awake()
    {
        _material = new Material(referenceMaterial);

        //_material.mainTexture = spriteRenderer.sprite.texture;
        //_material.mainTextureScale = new Vector2(widthSprite / widthImage, heightSprite / heightImage);

        meshRenderer.material = _material;
    }

    void Update()
    {
        //_material.mainTextureOffset = new Vector2((xBaseOffset + spriteRenderer.sprite.rect.x) / widthImage, (yBaseOffset + spriteRenderer.sprite.rect.y) / heightImage);
        
        meshRenderer.material.mainTexture = spriteRenderer.sprite.texture;
        if (_spinRemainingDuration > 0)
        {
            transform.RotateAround(transform.position, Vector3.up, - Time.deltaTime * 360 * _numberOfSpin / _spinTotalDuration);
            _spinRemainingDuration -= Time.deltaTime;
        }
    }

    public void Spin(float duration, int numberOfSpin)
    {
        if (_spinRemainingDuration <= 0)
        {
            _spinRemainingDuration = duration;
            _spinTotalDuration = duration;
            _numberOfSpin = numberOfSpin;

            StartCoroutine(EndSpin());
        }
    }

    private IEnumerator EndSpin()
    {
        yield return new WaitForSeconds(_spinTotalDuration + 0.05f);
        
        transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.back);
    }
}
