using System;
using System.Collections;
using UnityEngine;

public class PlatineDisk : BaseMovingItem
{
    [SerializeField] private BasicAttack shockWave;
    [SerializeField] private int damage;
    
    [HideInInspector] public float platineTime;
    private Transform _playerMeshTransform;
    private Transform _playerTransform;
    private Direction _currentDirection;
    enum Direction
    {
        UpRight,
        DownRight,
        DownLeft,
        UpLeft
    }
    
    public void Start()
    {
        StartCoroutine(StartMixing(platineTime));
        _playerMeshTransform = player.GetMesh().transform;
        _playerTransform = player.transform;
        _currentDirection = GetDir(GetDir3D());
    }

    private void Update()
    {
        Vector3 dir3D = GetDir3D();
        
        _playerMeshTransform.rotation = Quaternion.LookRotation(Vector3.down, dir3D);
        
        Direction newDirection = GetDir(dir3D);
        if (dir3D.magnitude > 0.3f 
            && (newDirection == _currentDirection + 1 
                || (_currentDirection == Direction.UpLeft && newDirection == Direction.UpRight)))
        {
            BasicAttack newShockWave = Instantiate(shockWave, transform.position + 0.5f * Vector3.up, Quaternion.identity);
            newShockWave.timeToLive = 0.1f;
            newShockWave.damage = damage;
            newShockWave.gameObject.SetActive(true);
            Destroy(newShockWave.gameObject, 0.15f);
            
            // SFX
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Game/Scratch");
        }
        
        _currentDirection = newDirection;
    }

    private IEnumerator StartMixing(float timeToMix)
    {
        player.TogglePlayerMovement(false);
        player.GetAnimator().SetBool(PlayerScript.IsAttackingAnimator, true);
        yield return new WaitForSeconds(timeToMix);
        player.TogglePlayerMovement(true);
        player.GetAnimator().SetBool(PlayerScript.IsAttackingAnimator, false);
        _playerMeshTransform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.back);
        Destroy(gameObject);
    }

    private Vector3 GetDir3D()
    {
        Vector2 input = GetInput();
        
        Vector3 truc = isKeyboardOrMouse ? 
            (new Vector3(input.x, 0, input.y) - _playerTransform.position) : //pour proportion pour check apres newDirection
            new Vector3(input.x, 0, input.y);

        //print("truc : " + truc + "; isKeyboardOrMouse " + isKeyboardOrMouse + "; _playerMeshTransform.position " + _playerTransform.position + "; input " + input);
        
        return truc;
    }

    private Direction GetDir(Vector3 dir3D)
    {
        return (dir3D.x > 0) ? (dir3D.z > 0 ? Direction.UpRight : Direction.DownRight) : (dir3D.z > 0 ? Direction.UpLeft : Direction.DownLeft);
    }
}
