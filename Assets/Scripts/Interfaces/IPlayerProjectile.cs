using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerProjectile 
{

    public void ProjectileHitEnemy();
    public void HandleScannerDetectedSomething(Collider2D scannedObject);


}
