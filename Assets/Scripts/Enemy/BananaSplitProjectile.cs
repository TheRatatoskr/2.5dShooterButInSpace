using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSplitProjectile : EnemyBasicProjectile
{

    [SerializeField] private Transform _bananaSprite;
    
    [SerializeField] private bool _isLeftBanana;

    [SerializeField] private float _arcSpeed = .1f;
    [SerializeField] private float _maxCrabSpeed = 2f;
    private float _currentArcSpeed = 0;

    [SerializeField] private float _bananaRotateSpeed;

    private void Start()
    {
        if(_isLeftBanana)
        {
            _arcSpeed = _arcSpeed * -1;
            _maxCrabSpeed = _maxCrabSpeed * -1;
        }

    }

    private void Update()
    {
        RotateBananaSprite();

        ProjectileMovement();
    }

    private void RotateBananaSprite()
    {
        _bananaSprite.Rotate(Vector3.forward * Time.deltaTime * _bananaRotateSpeed);
    }

    public override void ProjectileMovement()
    {
        
        _currentArcSpeed += _arcSpeed;
        if(Math.Abs(_currentArcSpeed) > Math.Abs(_maxCrabSpeed))
        {
            _currentArcSpeed = _maxCrabSpeed;
        }

        transform.Translate(new Vector2(_currentArcSpeed, _moveSpeed) * Time.deltaTime);
    }

}
