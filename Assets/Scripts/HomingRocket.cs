using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocket : MonoBehaviour, IPlayerProjectile
{
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _shotTime = 15f;
    [SerializeField] private float _damageDealt = 1f;

    [SerializeField] private GameObject _scanner;
    [SerializeField] private GameObject _explosion;

    private bool _enemyDetected = false;

    private GameObject _detectedEnemy;

    private void Start()
    {

        Destroy(this.gameObject, _shotTime);

    }
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_detectedEnemy == null)
        {
            _enemyDetected = false;
        }

        if (_enemyDetected)
        {

            transform.position = Vector2.MoveTowards(transform.position, _detectedEnemy.transform.position, _moveSpeed * Time.deltaTime);

            Vector3 direction = transform.position - _detectedEnemy.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Euler(0f, 0f, targetRotation.eulerAngles.z);
        }
        else
        {
            transform.Translate(transform.up * _moveSpeed * Time.deltaTime);
        }

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
        Instantiate(_explosion, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    public void ProjectileReachedMaxDistance()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void HandleScannerDetectedSomething(Collider2D scannedObject)
    {
        Debug.Log("made it to the call");
        if (scannedObject.tag == "Enemy")
        {
            

            _detectedEnemy = scannedObject.gameObject;
            _enemyDetected = true;
        }
    }
}
