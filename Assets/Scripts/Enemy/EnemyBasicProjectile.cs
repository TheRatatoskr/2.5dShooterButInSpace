using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicProjectile : MonoBehaviour
{
    [SerializeField] public float _moveSpeed;
    [SerializeField] private float _shotTime;
    [SerializeField] private int _damageDealt = 1;


    private void Start()
    {
        Destroy(this.gameObject, _shotTime);
    }
    void Update()
    {
        ProjectileMovement();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(_damageDealt); }
                ProjectileHitPlayer();
                break;
            case "PlayerProjectile":
                ProjectileHitPlayer();
                break;
            default:
                break;
        }
    }

    public virtual void ProjectileHitPlayer()
    {
        Destroy(this.gameObject);
    }

    public virtual void ProjectileMovement()
    {
        transform.Translate(-transform.up * _moveSpeed * Time.deltaTime);
    }

}
