using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bananarang : MonoBehaviour, IPlayerProjectile
{
    [SerializeField] private Transform _bananaSprite;
    [SerializeField] private float _bananaRotateSpeed = 5f;

    private bool _isReturning = false;
    private GameObject _playerObject;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _returnTime;
    private float _startingTime;
    [SerializeField] private float _returnSpeed;

    [SerializeField] private float _damageDealt;

    private void Start()
    {

        _startingTime = Time.time;

    }
    void Update()
    {
        transform.Translate(transform.up * _moveSpeed * Time.deltaTime);

        if(_startingTime + _returnTime < Time.time && !_isReturning)
        {
            _isReturning = true;
        }

        if(_isReturning)
        {
            ReturningToPlayer();
        }
        
        _bananaSprite.Rotate(Vector3.forward * Time.deltaTime * _bananaRotateSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                ProjectileHitEnemy();
                break;

            case "Player":
                if (_isReturning)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    _playerObject = collision.gameObject;
                }
                break;
            default:
                Debug.Log("banana hit unknown item");
                break;

        }
    }

    public void ProjectileHitEnemy()
    {
        //laugh in banana
    }

    public void ProjectileReachedMaxDistance()
    {
        //not implemented in banana
    }

    private void ReturningToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerObject.transform.position, _moveSpeed * _returnSpeed * Time.deltaTime);

    }
}
