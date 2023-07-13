using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPU : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _despawnYLocation;
    [SerializeField] private float _despawnTime;
    private float _spawnTime;

    [SerializeField] private int _whoAmI;

    private void Start()
    {
        _spawnTime = Time.time;
    }
    private void Update()
    {
        transform.Translate(new Vector2(0, _moveSpeed * Time.deltaTime));

        if (transform.position.y < _despawnYLocation)
        {
            Destroy(this.gameObject);
        }

        if (Time.time > _spawnTime + _despawnTime)
        {
            PowerUpDespawn();
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
}
