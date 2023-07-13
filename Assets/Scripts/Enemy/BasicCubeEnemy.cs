using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCubeEnemy : MonoBehaviour, IEnemy
{
    [Header("Basic Movement")]
    [SerializeField] private float _moveSpeed = 1f;
    
    [Header("Left and Right movement variance")]
    [SerializeField] private float _crabSpeed = 1f;
    [SerializeField] private float _maxShuffle = 1f;

    [Header("Top and Bottom bounds")]
    [SerializeField] private float _respawningLocation = 1f;
    [SerializeField] private float _maxDownDistance = 1f;

    [Header("Left and Right bounds")]
    [SerializeField] private float _respawnLeftBound;
    [SerializeField] private float _respawnRightBound;

    [Header("Stats")]
    [SerializeField] private int _contactDamage = 1;

    private SpawnManager _spawnManager;

    private bool _isAlive = true;

    [Header("Animation Controls")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _destroyObjectDelay = 2.8f;

    private AudioSource _audioSource;

    [Header("Sound Stuff")]
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioClip _laserNoises;

    [Header("Projectiles")]
    [SerializeField] private GameObject _laser;
    [SerializeField] private List<Vector3> _fireLocations;
    [SerializeField] private GameObject _laserBox;
    [SerializeField] private float _fireRate;
    private float _canFire = -1f;
    private bool _stopShooting = false;


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
        //impletement on other objects. 
    }



    public void HandleEnemyDeath()
    {
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

    private void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("No clip got assigned to enemy audio source");
            return;
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void RandomShooting()
    {
        if (_stopShooting) { return; }

        GameObject ShootyLaser = Instantiate(_laser, transform.position + _fireLocations[Random.Range(0, 1)], Quaternion.identity);

        ShootyLaser.transform.parent = _laserBox.transform;

        PlayAudio(_laserNoises);
    }
}
