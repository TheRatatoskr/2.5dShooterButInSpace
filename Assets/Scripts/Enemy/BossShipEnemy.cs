using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShipEnemy : BasicCubeEnemy
{
    [Header("Minion Spawning")]
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<GameObject> _spawnableDoods;
    [SerializeField] private float _spawnSpeed = 5f;

    [SerializeField] private List<GameObject> _projectilesToFire;

    [Header("Entrance Sequence")]
    [SerializeField] private Vector3 _spawnPosition;

    [Header("Death Sequence")]
    [SerializeField] private GameObject _bigExplosion;
    [SerializeField] private GameObject _smallExplosion;

    [Header("Health and Flash")]
    [SerializeField] private float _bossHealth;
    [SerializeField] private GameObject _bossMaterialObject;
    private Material _bossMaterial;
    [SerializeField] private Color _damageFlashColor;
    [SerializeField] private float _flashlength = .1f;
    private Color _baseColor;

    private void Start()
    {
        _bossMaterial = _bossMaterialObject.GetComponent<Renderer>().material;
        _baseColor = _bossMaterial.color;
        StartCoroutine(SpawnSomeAds());
    }

    public override void HandleEnemyDeath()
    {
        if (_isShielded)
        {
            _isShielded = false;
            _shieldSprite.SetActive(false);

            return;
        }
        if (_bossHealth > 0)
        {
            _bossHealth--;
            StartCoroutine(FlashBossOnDamage());
            return;
            
        }

        // disable laser so we dont shoot after death.
        _stopShooting = true;

        //stop collisions
        Destroy(GetComponent<Collider2D>());

        //make big boom happen
        //_anim.SetTrigger("OnEnemyWentBoom");

        _isAlive = false;

        PlayAudio(_explosion);

        //0 out stats
        _contactDamage = 0;
        _moveSpeed = 0f;
        _crabSpeed = 0f;

        _spawnManager.AMurderWasReported();

        Destroy(this.gameObject, _destroyObjectDelay);
    }

    public override void RandomShooting()
    {
    //    if (_stopShooting) { return; }

    //    GameObject shootyLaser = Instantiate(_laser, transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

    //    shootyLaser.transform.parent = _laserBox.transform;

    //    PlayAudio(_laserNoises);
    }

    public override void HandleEnemyMovement()
    {
        
    }

    IEnumerator FlashBossOnDamage()
    {
        _bossMaterial.color = _damageFlashColor; 
        yield return new WaitForSeconds(_flashlength);
        _bossMaterial.color = _baseColor;

    }

    IEnumerator SpawnSomeAds()
    {
        while (!_stopShooting)
        {

            Transform spawnLocation = _spawnPoints[Random.Range(0, _spawnPoints.Count - 1)];

            yield return new WaitForSeconds(_spawnSpeed);
            
            GameObject newDood = Instantiate(_spawnableDoods[Random.Range(0, _spawnableDoods.Count)], spawnLocation.position, Quaternion.identity);
            //newDood.transform.parent = _spawnManager.gameObject.transform.GetChild(0);

            
        }
    }
}
