using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingSpeedEnemy : BasicCubeEnemy
{
    [SerializeField] private GameObject _ramZone;
    private Transform _playerLocation;
    private bool _isRamming = false;

    private Quaternion _defaultPlayerRotation;

    private void Start()
    {
        _defaultPlayerRotation = Quaternion.identity;
    }
    public override void HandleScannerDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "Player")
        {
            _playerLocation = scannedObject.transform;
            _isRamming = true;
        }
    }

    public override void HandleScannerLostDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "Player")
        {
            _isRamming = false;
        }
    }

    public override void HandleEnemyMovement()
    {
        if (_isRamming)
        {
            

            transform.position = Vector2.MoveTowards(transform.position, _playerLocation.position, _moveSpeed * Time.deltaTime);

            //calculate turning to face the player.

            Vector3 direction = transform.position - _playerLocation.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation.eulerAngles.z);
        }
        else
        {
            base.HandleEnemyMovement();
            transform.rotation = _defaultPlayerRotation;
        }
        
        
    }

}
