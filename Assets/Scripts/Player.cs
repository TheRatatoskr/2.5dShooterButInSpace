using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player transform settings

    [Header("Change How the player moves")]
    [Tooltip("Define the starting position of the player.")]
    [SerializeField] private Vector3 _startingPosition;

    [Tooltip("Define how fast the player moves.")]
    [SerializeField] 
    
    private float _moveSpeed = 3.5f;
    
    [Space(order = 1)]
    [SerializeField] private float randomMinimum = 0f;
    [SerializeField] private float randomMaximum = 10f;

    [Header("Player Clamping")]
    [SerializeField] private float yPositionMin = -3.8f;
    [SerializeField] private float yPositionMax = 0;
    [Space(order = 1)]
    [SerializeField] private float xPositionMin = -3.8f;
    [SerializeField] private float xPositionMax = 0;

    [Header("Projectile Handling")]
    [Tooltip("Object that the player currently fires.")]
    [SerializeField] private GameObject _playerProjectile;
    [Tooltip("Speed that the projectile moves at.")]
    [SerializeField] private float _projectileSpeed;
    [Tooltip("How high up the projectile can travel")]
    [SerializeField] private float _highestYPoint;
    [Tooltip("This changes the insatiate location for projects relative to the player")]
    [SerializeField] private Vector3 _projectileStartOffset;

    [Tooltip("How fast the player can shoot")]
    [SerializeField] private float _fireRate = .5f;
    private float _canFire =-1f;

    [Header("Stats")]
    [SerializeField] private float _playerHealth = 5f;

    private SpawnManager _spawnManager;

    private Vector2 _currentMovement;

    [SerializeField] private InputReader inputReader;

    private void Start()
    {
        Debug.Log("Start called and events are subed");
        inputReader.MovementKeysChanged += ChangeMovement;
        inputReader.PrimaryFirePressed += FirePlayerProjectile;

        transform.position = _startingPosition;
    }
    private void ChangeMovement(Vector2 newMovement)
    {
        _currentMovement = newMovement;
    }

    private void Update()
    {
        CalculateMovement();

    }
    public void InitializePlayer(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;

    }
    private void CalculateMovement()
    {
        
        //clamp y movement
        transform.position = new Vector2( transform.position.x, Mathf.Clamp(transform.position.y, yPositionMin, yPositionMax));

        //wrap x movement
        if (transform.position.x >= xPositionMax)
        {
            transform.position = new Vector2(xPositionMin, transform.position.y);
        }
        else if (transform.position.x <= xPositionMin)
        {
            transform.position = new Vector2(xPositionMax, transform.position.y);
        }



        Debug.Log("I see joystick input as " + _currentMovement.ToString());
        //do the move
        transform.Translate(_currentMovement * _moveSpeed * Time.deltaTime);
    }


    private void FirePlayerProjectile(bool buttonIsPressed)
    {
        if (buttonIsPressed && Time.time > _canFire)
        {
            //null check
            if (_playerProjectile == null)
            {
                Debug.LogWarning("Player is trying to fire a projectile, but theres no prefab. Do you need a break?");
                return;
            }

            //assign current time + delay
            _canFire = Time.time + _fireRate;

            GameObject playerProjectile = Instantiate(_playerProjectile,
                                                          transform.position + _projectileStartOffset,
                                                          Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collision:" + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "Enemy":
                Debug.Log("Enemy Tag");
                break;
            case "Player":
                Debug.Log("Player Tag");
                break;
            case "PlayerProjectile":
                Debug.Log("PlayerProjectile Tag");
                break;
            case "EnemyProjectile":
                Debug.Log("EnemyProjectile Tag");
                break;
            default:
                Debug.Log(gameObject.name + " collided with something thats untagged");
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        GetComponent<AudioSource>().Play();
        _playerHealth = _playerHealth-damage;
        if( _playerHealth <= 0 )
        {
            PlayerDeath();
        }

    }
    private void PlayerDeath()
    {
        _spawnManager.StopSpawningDoods();
        Destroy(this.gameObject);
    }

}

