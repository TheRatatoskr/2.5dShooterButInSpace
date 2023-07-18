
using UnityEngine;

public interface IEnemy
{
    public void InitializeEnemy(SpawnManager spawnManager);
    public void HandleEnemyMovement();
    public void HandleScreenExit();

    public void HandleEnemyDeath();

    public void ActivateShields();

    public void HandleScannerDetection(Collider2D collision);
    public void HandleScannerLostDetection(Collider2D collision);

    public void RandomShooting();
}
