using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCubeEnemy : MonoBehaviour, IEnemy
{
    [Header("Basic Movement")]
    [SerializeField] protected float _moveSpeed = 4f;
    
    [Header("Left and Right movement variance")]
    [SerializeField] protected float _crabSpeed = 1f;
    [SerializeField] protected float _maxShuffle = 1f;

    [Header("Top and Bottom bounds")]
    [SerializeField] protected float _respawningLocation = 10f;
    [SerializeField] protected float _maxDownDistance = -8f;

    [Header("Left and Right bounds")]
    [SerializeField] protected float _respawnLeftBound = -10f;
    [SerializeField] protected float _respawnRightBound = 10f;

    [Header("Stats")]
    [SerializeField] protected int _contactDamage = 1;

    protected SpawnManager _spawnManager;

    protected bool _isAlive = true;

    [Header("Animation Controls")]
    [SerializeField] protected Animator _anim;
    [SerializeField] protected float _destroyObjectDelay = 2.8f;

    protected AudioSource _audioSource;

    [Header("Sound Stuff")]
    [SerializeField] protected AudioClip _explosion;
    [SerializeField] protected AudioClip _laserNoises;

    [Header("Projectiles")]
    [SerializeField] protected GameObject _laser;
    [SerializeField] protected List<Vector3> _fireLocations;
    [SerializeField] protected GameObject _laserBox;
    [SerializeField] protected float _fireRate = 4f;
    protected float _canFire = -1f;
    protected bool _stopShooting = false;

    protected bool _isShielded = false;
    [SerializeField] protected GameObject _shieldSprite;

    private void Update()
    {
        if (_isAlive)
        {
            HandleEnemyMovement();
        }

        HandleScreenExit();

        if (_canFire < Time.time)
        {
            _canFire = Time.time + _fireRate;
            RandomShooting();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collision:" + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "Player":
                Player player = other.gameObject.GetComponent<Player>();
                if (player != null) { player.TakeDamage(_contactDamage); }
                HandleEnemyDeath();
                break;

            case "PlayerProjectile":
                IPlayerProjectile playerProjectile = other.gameObject.GetComponent<IPlayerProjectile>();
                if (playerProjectile != null) { playerProjectile.ProjectileHitEnemy(); }
                HandleEnemyDeath();
                break;

            default:
                Debug.Log(gameObject.name + " collided with something thats untagged");
                break;
        }
    }


    public void InitializeEnemy(SpawnManager spawnManager)
    {
        _spawnManager = spawnManager;

        _audioSource = GetComponent<AudioSource>();

        _laserBox = spawnManager.transform.GetChild(2).gameObject;

    }

    public void ActivateShields()
    {
        _isShielded = true;
        _shieldSprite.SetActive(true);
    }

    public virtual void HandleEnemyMovement()
    {
        //calculate the left or right shuffle
        float shuffle = Random.Range(-1 * _maxShuffle, _maxShuffle);
        float xMotion = _crabSpeed * shuffle * Time.deltaTime;

        float yMotion = -_moveSpeed * Time.deltaTime;

        //do the move
        transform.Translate(new Vector3(xMotion,yMotion,0));

    }

    public void HandleScreenExit()
    { 
        if(transform.position.y < _maxDownDistance)
        {

            //place enemy at location off of the top of the screen
            transform.position = new Vector3(
                Random.Range(_respawnLeftBound, _respawnRightBound), _respawningLocation, transform.position.z);

            HandleAdditionalExitBehaviors();

        }
    }

    public virtual void HandleAdditionalExitBehaviors()
    {
        //implement on other objects. 
    }



    public virtual void HandleEnemyDeath()
    {
        if(_isShielded)
        {
            _isShielded = false;
            _shieldSprite.SetActive(false);

            return;
        }

        // disable laser so we dont shoot after death.
        _stopShooting = true;

        //stop collisions
        Destroy(GetComponent<Collider2D>());

        //make big boom happen
        _anim.SetTrigger("OnEnemyWentBoom");

        _isAlive = false;

        PlayAudio(_explosion);
        
        //0 out stats
        _contactDamage = 0;
        _moveSpeed = 0f;
        _crabSpeed = 0f;

        _spawnManager.AMurderWasReported();

        Destroy(this.gameObject, _destroyObjectDelay);
    }

    protected void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("No clip got assigned to enemy audio source");
            return;
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public virtual void RandomShooting()
    {
        if (_stopShooting) { return; }

        GameObject shootyLaser = Instantiate(_laser, transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

        shootyLaser.transform.parent = _laserBox.transform;

        PlayAudio(_laserNoises);
    }

    public virtual void HandleScannerDetection(Collider2D collision)
    {
        //not implemented on basic enemy.
    }

    public virtual void HandleScannerLostDetection(Collider2D collision)
    {
        //not implemented on basic enemy.
    }
}
