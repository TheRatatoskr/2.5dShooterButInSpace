using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackwardsShootyEnemy : BasicCubeEnemy
{
    private bool _playerIsBehindUs = false;

    [SerializeField] private GameObject _backwardsLaser;

    [SerializeField] private GameObject _cannonSprite1;
    [SerializeField] private GameObject _cannonSprite2;

    public override void HandleScannerDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "Player")
        {
            _playerIsBehindUs = true;
            RandomShooting();
        }
    }

    public override void HandleScannerLostDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "Player")
        {
            _playerIsBehindUs = false;
        }

    }

    public override void RandomShooting()
    {
        if (_playerIsBehindUs)
        {
            if (_stopShooting) { return; }

            GameObject ShootyLaser = Instantiate(_backwardsLaser, transform.position + -_fireLocations[Random.Range(0, 1)], Quaternion.identity);

            ShootyLaser.transform.parent = _laserBox.transform;

            base.PlayAudio(_laserNoises);
        }
        else
        {
            base.RandomShooting();
        }
        
    }
    public override void HandleEnemyDeath()
    {
        _cannonSprite1.SetActive(false);
        _cannonSprite2.SetActive(false);

        base.HandleEnemyDeath();

    }

}
