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
    [SerializeField] private int _contactDamage = 1;

    private SpawnManager _spawnManager;

    [Header("Animation Controls")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _destroyObjectDelay = 2.8f;
    

    private void Update()
    {
        HandleEnemyMovement();
        HandleScreenExit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collision:" + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "Enemy":

                break;
            case "Player":


                Player player = other.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(_contactDamage); }
                HandleEnemyDeath();

                break;
            case "PlayerProjectile":

                
                IPlayerProjectile playerProjectile = other.gameObject.GetComponent<IPlayerProjectile>();
                if (playerProjectile != null) { playerProjectile.ProjectileHitEnemy(); }
                HandleEnemyIsShotByPlayer();

                break;
            case "EnemyProjectile":

                break;
            default:
                Debug.Log(gameObject.name + " collided with something thats untagged");
                break;
        }
    }


    public void InitializeEnemy(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;
    }

    public void HandleEnemyMovement()
    {
        //calculate the left or right shuffle
        float shuffle = Random.Range(-1 * _maxShuffle, _maxShuffle);
        float xMotion = _crabSpeed * shuffle * Time.deltaTime;

        float yMotion = -_moveSpeed * Time.deltaTime;

        //do the move
        transform.Translate(new Vector3(xMotion,yMotion,0));

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

    public void HandleEnemyIsShotByPlayer()
    {
        _spawnManager.AMurderWasReported();
        HandleEnemyDeath();
    }

    public void HandleEnemyDeath()
    {
        _anim.SetTrigger("OnEnemyWentBoom");
        _contactDamage = 0;
        _moveSpeed = 0f;
        _crabSpeed = 0f;
        Destroy(this.gameObject, _destroyObjectDelay);
    }
}
