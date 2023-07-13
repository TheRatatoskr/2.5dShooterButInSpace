using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixEnemy : BasicCubeEnemy
{
    [Header("Helix Enemy Settings")]
    [SerializeField] private float _flightSpeed = 4f;
    [SerializeField] private float _sideSpeedMax = 4f;
    [SerializeField] private float _sideSpeedAccel = 0.01f;

    private bool _swapTime = false;

    private float _currentSideMotion;

    public override void HandleEnemyMovement()
    {
        if (_swapTime)
        {
            _sideSpeedAccel = _sideSpeedAccel * -1;
            _swapTime = false;

        }

        _currentSideMotion += _sideSpeedAccel;

        if (_currentSideMotion >= _sideSpeedMax || _currentSideMotion <= -_sideSpeedMax) 
        {
            _swapTime = true;
        }

        float xMotion = _currentSideMotion * Time.deltaTime;
        float yMotion = -_flightSpeed * Time.deltaTime;

        //do the move
        transform.Translate(new Vector3(xMotion, yMotion, 0));

    }
}
