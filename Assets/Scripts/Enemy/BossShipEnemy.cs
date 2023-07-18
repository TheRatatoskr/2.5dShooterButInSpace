using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShipEnemy : BasicCubeEnemy
{
    [Header("Minion Spawning")]
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<GameObject> _spawnableDoods;
    [SerializeField] private float _spawnSpeed = 5f;

    [Header("Shooty Bits")]
    [SerializeField] private List<GameObject> _projectilesToFire;
    [SerializeField] private GameObject _shootSpot1;
    [SerializeField] private GameObject _shootSpot2;

    [Header("Entrance Sequence")]
    [SerializeField] private Vector3 _startDestination;
    private bool _canBeDamaged = false;

    [Header("Death Sequence")]
    [SerializeField] private GameObject _bigExplosion;
    [SerializeField] private GameObject _smallExplosion;
    [SerializeField] private Transform _bottomLeftCorner;
    [SerializeField] private Transform _topRightCorner;
    [SerializeField] private float _explosionTime = 10f;


    [Header("Health and Flash")]
    [SerializeField] private float _bossHealth;
    [SerializeField] private GameObject _bossMaterialObject;
    private Material _bossMaterial;
    [SerializeField] private Color _damageFlashColor;
    [SerializeField] private float _flashlength = .1f;
    private Color _baseColor;

    
    

    private void Start()
    {
        _isAlive = false;
        _stopShooting = true;
        _bossMaterial = _bossMaterialObject.GetComponent<Renderer>().material;
        _baseColor = _bossMaterial.color;
        StartCoroutine(SlowDramaticEntrance());
    }

    public override void HandleEnemyDeath()
    {
        if (!_canBeDamaged) 
        { 
            return; 
        }

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

        //_spawnManager.StopTheBossWave();


        StartCoroutine(ExplosionSequence());

        _isAlive = false;

        //0 out stats
        _contactDamage = 0;
        _moveSpeed = 0f;
        _crabSpeed = 0f;
        
    }

    public override void RandomShooting()
    {
        Debug.Log("Boss tried to shoot");
    if (_stopShooting) { return; }

        GameObject shootyLaser1 = Instantiate(_projectilesToFire[Random.Range(0, _projectilesToFire.Count - 1)], 
            _shootSpot1.transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

        GameObject shootyLaser2 = Instantiate(_projectilesToFire[Random.Range(0, _projectilesToFire.Count - 1)], 
            _shootSpot2.transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

        shootyLaser1.transform.parent = _laserBox.transform;
        shootyLaser2.transform.parent = _laserBox.transform;

        //PlayAudio(_laserNoises);
    }

    public override void HandleEnemyMovement()
    {

        if (Mathf.Abs(transform.position.x) >= _respawnLeftBound)
        {
            _crabSpeed = _crabSpeed * -1f;
        }

        transform.Translate(new Vector3(_crabSpeed * Time.deltaTime, 0, 0));
    }

    IEnumerator SlowDramaticEntrance()
    {
        // warning flash

        while (transform.position != _startDestination)
        {
            transform.position = Vector2.MoveTowards(transform.position, _startDestination, _moveSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _isAlive = true;
        _canBeDamaged = true;
        _stopShooting = false;
        StartCoroutine(SpawnSomeAds());
 
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
            newDood.transform.parent = _spawnManager.gameObject.transform.GetChild(0);
            


        }
    }

    IEnumerator ExplosionSequence()
    {
        float alarmTime = Time.time + _explosionTime;
        while (alarmTime > Time.time)
        {
            float xPosition = Random.Range(_bottomLeftCorner.position.x, _topRightCorner.position.x);
            float yPosition = Random.Range(_bottomLeftCorner.position.y, _topRightCorner.position.y);

            Instantiate(_smallExplosion, new Vector3(xPosition,yPosition,-.2f), Quaternion.identity);
            yield return new WaitForEndOfFrame();

            
        }
        _spawnManager.StopTheBossWave();
        Destroy(this.gameObject);

    }
}
