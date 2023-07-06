using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazer : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _shotTime;
    private float _isDead = -1f;
    [SerializeField]
    private int _damageDealt = 1;

    private float _startingPosition;


    private void Start()
    {
        Destroy(this.gameObject, _shotTime);
        _startingPosition = transform.position.y;
    }
    void Update()
    {
        transform.Translate(-transform.up * _moveSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player"://not implemented
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(_damageDealt); }
                ProjectileHitPlayer();
                break;
            case "Wall"://not implemented

                break;
            default:

                break;

        }
    }

    public void ProjectileHitPlayer()
    {
        Destroy(this.gameObject);
    }

}
