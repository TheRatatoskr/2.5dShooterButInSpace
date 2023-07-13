using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void InitializeEnemy(SpawnManager spawnManager);
    public void HandleEnemyMovement();
    public void HandleScreenExit();

    public void HandleEnemyDeath();
}
