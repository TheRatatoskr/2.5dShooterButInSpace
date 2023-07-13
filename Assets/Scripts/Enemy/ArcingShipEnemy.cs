using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingShipEnemy : BasicCubeEnemy
{
    [Header("Arcing Enemy Movement")]
    [SerializeField] private float _flightSpeed = 1f;

    [Header("Arcing Variance")]

    [SerializeField] private float _maxSideSpeed = .3f;

    [SerializeField] private float _arcSpeed = .1f;

    [SerializeField] private bool _isLeft = false;

    private float _sideSpeed = 0f;

    private void Start()
    {
        int leftOrRight = Random.Range(0, 1);
        if(leftOrRight == 0)
        {
            _isLeft = true;
        }
        else
        {
            _isLeft = false;
        }
    }

    
    public override void HandleEnemyMovement()
    {
        //check direction and apply adjustment
        if (_isLeft)
        {
            _sideSpeed = Mathf.Clamp(_sideSpeed - _arcSpeed, -_maxSideSpeed, _maxSideSpeed);
        }
        else
        {
            _sideSpeed = Mathf.Clamp(_sideSpeed + _arcSpeed, -_maxSideSpeed, _maxSideSpeed);
        }


        float xMotion = _sideSpeed * Time.deltaTime;
        float yMotion = -_flightSpeed * Time.deltaTime;

        //do the move
        transform.Translate(new Vector3(xMotion, yMotion, 0));

    }

    public override void HandleAdditionalExitBehaviors()
    {
        if(_isLeft)
        {
            _isLeft = false;
        }
        else
        {
            _isLeft = true;
        }
    }

}
