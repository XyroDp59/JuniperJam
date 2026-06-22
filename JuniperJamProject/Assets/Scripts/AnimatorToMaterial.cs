using System.Collections;
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
    
    private float _spinRemainingDuration = 0;
    private float _spinTotalDuration = 0;
    private int _numberOfSpin = 1;

    private void Awake()
    {
        _material = new Material(Shader.Find("HDRP/Lit"));
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
