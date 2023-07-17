using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootyTrollyEnemy : BasicCubeEnemy
{
    private bool _aimAtPowerUp = false;

    private GameObject _powerUpObject;


    public override void HandleScannerDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "PlayerPowerUp")
        {
            _powerUpObject = scannedObject.gameObject;
            _aimAtPowerUp = true;
            RandomShooting();
        }
    }

    public override void HandleScannerLostDetection(Collider2D scannedObject)
    {
        if (scannedObject.tag == "PlayerPowerUp")
        {

        }
    }

    public override void RandomShooting()
    {
        if (_stopShooting) { return; }

        GameObject shootyLaser = Instantiate(_laser, transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

        if (_aimAtPowerUp)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, transform.position - _powerUpObject.transform.position);
            shootyLaser.transform.rotation = Quaternion.Euler(0f, 0f, targetRotation.eulerAngles.z);
            _aimAtPowerUp = false;
        }

        shootyLaser.transform.parent = _laserBox.transform;

        PlayAudio(_laserNoises);
    }
}
