using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPowerPU : MonoBehaviour, IPowerUp
{
    [SerializeField] protected float _moveSpeed = 4f;
    [SerializeField] protected float _moveSpeedMultiplier = 2f;

    [SerializeField] protected float _despawnYLocation;
    [SerializeField] protected float _despawnTime;
    protected float _spawnTime;
    protected Vector3 _playerCallPosition;


    [SerializeField] protected int _whoAmI;

    protected bool _goToPlayer = false;


    private void Start()
    {
        _spawnTime = Time.time;
    }
    private void Update()
    {
        HandlePowerUpMovement();

        if (transform.position.y < _despawnYLocation)
        {
            PowerUpDespawn();
        }

        if (Time.time > _spawnTime + _despawnTime)
        {
            PowerUpDespawn();
        }
    }

    public virtual void HandlePowerUpMovement()
    {
        if (_goToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerCallPosition, -_moveSpeed * _moveSpeedMultiplier * Time.deltaTime);
            if (transform.position == _playerCallPosition)
            {
                _goToPlayer = false;
            }
        }
        else
        {
            transform.Translate(new Vector2(0, _moveSpeed * Time.deltaTime));
        }
    }

    public int PlayerPickedMeUp()
    {
        return _whoAmI;
    }

    public void PowerUpDespawn()
    {
        Destroy(this.gameObject);
    }

    public void SendToPlayer(Vector3 playerPosition)
    {
        _playerCallPosition = playerPosition;
        _goToPlayer = true;

    }
}
