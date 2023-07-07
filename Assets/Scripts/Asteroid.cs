using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _explosion;

    private SpawnManager _spawnManager;

    public void InitializeAsteroid(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "PlayerProjectile":
                HandleDestruction();
                Destroy(collision.gameObject);
                break;
            default: break;

        }
    }

    public void HandleDestruction()
    {
        _spawnManager.StartSpawningDoods();
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject, .5f);
    }
}
