using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeyEnemy : BasicCubeEnemy
{
    private bool _isDodging = false;
    private Vector3 _projectileToDodge;

    [SerializeField] private float _dodgeSpeed = 2f;

    public override void HandleScannerDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "PlayerProjectile")
        {
            _isDodging = true;
            _projectileToDodge = scannedObject.transform.position;
        }
    }

    public override void HandleScannerLostDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "PlayerProjectile")
        {
            _isDodging = false;
        }
    }

    public override void HandleEnemyMovement()
    {
        if (_isDodging)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_projectileToDodge.x,0,0), -_dodgeSpeed * Time.deltaTime);
        }
        
        base.HandleEnemyMovement();

    }
}
