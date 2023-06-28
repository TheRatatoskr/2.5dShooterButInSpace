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
    [SerializeField] private float _moveSpeed = 3.5f;

    [SerializeField] private float randomMinimum = 0f;
    [SerializeField] private float randomMaximum = 10f;

    [Header("Player Clamping")]
    [SerializeField] private float yPositionMin = -3.8f;
    [SerializeField] private float yPositionMax = 0;
    [Space(order = 1)]
    [SerializeField] private float xPositionMin = -3.8f;
    [SerializeField] private float xPositionMax = 0;

    private void Start()
    {
        transform.position = _startingPosition;
    }
    private void Update()
    {
        CalculateMovement();
        


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
}
