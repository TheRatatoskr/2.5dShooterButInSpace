using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour, IPlayerProjectile
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _shotDistance;
    [SerializeField]
    private float _damageDealt;

    private float _startingPosition;

    public void ProjectileHitEnemy()
    {
        Destroy(this.gameObject);
    }

    public void ProjectileHitWall()
    {
        //future implementation
    }

    public void ProjectileReachedMaxDistance()
    {
        Destroy(this.gameObject);
    }


    private void Start()
    {
        _startingPosition = transform.position.y;
    }
    void Update()
    {
        transform.Translate(transform.up * _moveSpeed * Time.deltaTime);
        if (transform.position.y - _startingPosition > _shotDistance)
        {
            ProjectileReachedMaxDistance();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy"://not implemented
                ProjectileHitEnemy();
                break;
            case "Wall"://not implemented
                ProjectileHitWall();
                break;
            default:
                Debug.Log("Projectile hit unknown item");
                break;

        }
    }

}
