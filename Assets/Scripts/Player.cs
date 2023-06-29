using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        transform.position = _startingPosition;
    }
    private void Update()
    {
        CalculateMovement();
        FirePlayerProjectile();
    }

    private void CalculateMovement()
    {
        
        //clamp y movement
        transform.position = new Vector3( transform.position.x, Mathf.Clamp(transform.position.y, yPositionMin, yPositionMax));

        //wrap x movement
        if (transform.position.x >= xPositionMax)
        {
            transform.position = new Vector3(xPositionMin, transform.position.y);
        }
        else if (transform.position.x <= xPositionMin)
        {
            transform.position = new Vector3(xPositionMax, transform.position.y);
        }

        //Calculate a random movespeed on each frame.
        float spot = Random.Range(randomMinimum, randomMaximum);

        //determine the move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //do the move
        transform.Translate(direction * spot * Time.deltaTime);
    }
    private void FirePlayerProjectile()
    {
        if (_playerProjectile == null)
        {
            Debug.LogWarning("Player is trying to fire a projectile, but theres no prefab. Do you need a break?");
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            //make zappy zappy.
            //tell zappy to move up so many spots and die

        }



            GameObject playerProjectile = Instantiate(_playerProjectile, transform.position, Quaternion.identity);
            PlayerProjectile projectile = playerProjectile.GetComponent<PlayerProjectile>();
            projectile.Initialize(_projectileSpeed, _highestYPoint);

        }
    }
}
