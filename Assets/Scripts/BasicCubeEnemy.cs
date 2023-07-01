using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCubeEnemy : MonoBehaviour, IEnemy
{
    [Header("Basic Movement")]
    [SerializeField] private float _moveSpeed = 1f;
    
    [Header("Left and Right movement variance")]
    [SerializeField] private float _crabSpeed = 1f;
    [SerializeField] private float _maxShuffle = 1f;

    [Header("Top and Bottom bounds")]
    [SerializeField] private float _respawningLocation = 1f;
    [SerializeField] private float _maxDownDistance = 1f;

    [Header("Left and Right bounds")]
    [SerializeField] private float _respawnLeftBound;
    [SerializeField] private float _respawnRightBound;

    [Header("Stats")]
    [SerializeField] private float _contactDamage = 1f;

    //component variables


    public void HandleEnemyMovement()
    {
        //calculate the left or right shuffle
        float shuffle = Random.Range(-1 * _maxShuffle, _maxShuffle);

        //do the move
        transform.Translate(new Vector3(
            _crabSpeed * shuffle * Time.deltaTime,
            -_moveSpeed* Time.deltaTime,
            0));


    }
    public void HandleScreenExit()
    {
        if(transform.position.y < _maxDownDistance)
        {
            //place enemy at location off of the top of the screen
            transform.position = new Vector3(
                Random.Range(_respawnLeftBound, _respawnRightBound),
                _respawningLocation,
                transform.position.z);
        }
    }
    public void HandleDestroyEnemy()
    {
        Destroy(this.gameObject);

    }    
    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Collision:" + other.gameObject.name);
        switch(other.gameObject.tag)
        {
            case "Enemy":
                Debug.Log("Enemy Tag");
                break;
            case "Player": 
                Debug.Log("Player Tag");

                Player player = other.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(_contactDamage); }

                break;
            case "PlayerProjectile":
                Debug.Log("PlayerProjectile Tag");

                IPlayerProjectile playerProjectile = other.gameObject.GetComponent<IPlayerProjectile>();
                if (playerProjectile != null) { playerProjectile.ProjectileHitEnemy(); }

                HandleDestroyEnemy();

                break;
            case "EnemyProjectile":
                Debug.Log("EnemyProjectile Tag");
                break;
            default:
                Debug.Log(gameObject.name + " collided with something thats untagged");
                break;
        }
    }
    private void Update()
    {
        HandleEnemyMovement();
        HandleScreenExit();
    }

}
