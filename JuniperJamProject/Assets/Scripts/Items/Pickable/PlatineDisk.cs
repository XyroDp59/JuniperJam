using System;
using System.Collections;
using UnityEngine;

public class PlatineDisk : BaseMovingItem
{
    [HideInInspector] public float platineTime;
    private Transform _playerMeshTransform;
    
    public void Start()
    {
        StartCoroutine(StartMixing(platineTime));
        _playerMeshTransform = player.GetMesh().transform;
    }

    private void Update()
    {
        if (isKeyboardOrMouse)
            _playerMeshTransform.rotation = Quaternion.LookRotation(Vector3.down, new Vector3(GetInput().x, 0, GetInput().y));
        else
            _playerMeshTransform.rotation = Quaternion.LookRotation(Vector3.down,  new Vector3(GetInput().x, 0, GetInput().y) - _playerMeshTransform.position);
    }

    private IEnumerator StartMixing(float timeToMix)
    {
        player.TogglePlayerInput(false);
        yield return new WaitForSeconds(timeToMix);
        player.TogglePlayerInput(true);
        _playerMeshTransform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.back);
        Destroy(gameObject);
    }
}
