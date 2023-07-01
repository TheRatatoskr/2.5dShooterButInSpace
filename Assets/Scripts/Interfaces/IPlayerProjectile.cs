using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerProjectile 
{

    public void ProjectileHitEnemy();
    public void ProjectileHitWall();
    public void ProjectileReachedMaxDistance();

}