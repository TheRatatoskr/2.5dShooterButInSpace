using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour, IPlayerProjectile
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _shotTime;
    [SerializeField] private float _damageDealt;




    private void Start()
    {

        Destroy(this.gameObject, _shotTime);

    }
    void Update()
    {
        transform.Translate(transform.up * _moveSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                ProjectileHitEnemy();
                break;

            default:
                Debug.Log("Projectile hit unknown item");
                break;

        }
    }

    public void ProjectileHitEnemy()
    {
        Destroy(this.gameObject);
    }

    public void ProjectileReachedMaxDistance()
    {
        Destroy(this.gameObject);
    }


}
