using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalPU : BasicPowerPU
{
    
    [SerializeField] private float _minXPosition;
    [SerializeField] private float _maxXPosition;

    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _crabSpeed = 3f;

    private void Start()
    {
        int leftOrRight = Random.Range(0, 1);
        if(leftOrRight == 0)
        {
            _crabSpeed = _crabSpeed * -1f;
        }
    }
    public override void HandlePowerUpMovement()
    {
        transform.Translate(new Vector2(_crabSpeed * Time.deltaTime, _movementSpeed * Time.deltaTime));
        if (transform.position.x < _minXPosition || transform.position.x > _maxXPosition)
        {
            _crabSpeed = _crabSpeed * -1;
        }
    }
}
