using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp 
{
    public int PlayerPickedMeUp();
    public void PowerUpDespawn();
    public void SendToPlayer(Vector3 playerPosition);
}
