using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player transform settings

    [Header("Change How the player moves")]
    [Tooltip("Define the starting position of the player.")]
    [SerializeField] private Vector3 _startingPosition;

    private float _currentMoveSpeed = 3.5f;
    [Tooltip("Define how fast the player moves normally.")]
    [SerializeField] private float _normalMoveSpeed = 3.5f;
    [Tooltip("Define how fast the player moves speed up.")]
    [SerializeField] private float _boostedSpeed = 10f;



    [Space(order = 1)]
    [SerializeField] private float _randomMinimum = 0f;
    [SerializeField] private float _randomMaximum = 10f;

    [Header("Player Clamping")]
    [SerializeField] private float _yPositionMin = -3.8f;
    [SerializeField] private float _yPositionMax = 0;
    [Space(order = 1)]
    [SerializeField] private float _xPositionMin = -3.8f;
    [SerializeField] private float _xPositionMax = 0;

    [Header("Projectile Handling")]

    [Tooltip("Speed that the projectile moves at.")]
    [SerializeField] private float _projectileSpeed;
    [Tooltip("How high up the projectile can travel")]
    [SerializeField] private float _highestYPoint;
    [Tooltip("This changes the insatiate location for projects relative to the player")]
    [SerializeField] private Vector3 _projectileStartOffset;

    [Tooltip("Object that the player currently fires.")]
    [SerializeField] private GameObject _playerProjectile;

    [SerializeField] private List<GameObject> _projectileLists;
    private const int SINGLE_LASER = 0;
    private const int TRIPLE_LASER = 1;
    private const int SPEED_UP = 11;
    private const int SHIELD_UP = 21;
    private int _currentPower;

    [SerializeField] private GameObject _shieldSprite;
    private bool _isShielded = false;

    [Tooltip("How fast the player can shoot")]
    [SerializeField] private float _fireRate = .5f;
    private float _canFire =-1f;

    [Header("Stats")]
    [SerializeField] private int _playerHealth = 3;

    [Header("Player Damage Animations")]
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private GameObject _explosion;

    private bool _engineFireEnabled = false;
    private int _secondEngineFire;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [Header("Audio Objects")]
    [SerializeField] private AudioClip _laserNoises;
    [SerializeField] private AudioClip _explosionNoises;
    [SerializeField] private AudioClip _pickupNoise;

    private AudioSource _audioSource;


    private void Start()
    {
        transform.position = _startingPosition;

        _shieldSprite.gameObject.SetActive(false);

        _audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        CalculateMovement();

        if (Input.GetButtonDown("Jump") && Time.time > _canFire)
        {
            FirePlayerProjectile();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Collision:" + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "PlayerPowerUp":
                
                IPowerUp powerUp = other.gameObject.GetComponent<IPowerUp>();
                CollectedPowerUp(powerUp.PlayerPickedMeUp());
                powerUp.PowerUpDespawn();
                break;

            default:
                Debug.Log(gameObject.name + " collided with something thats untagged");
                break;
        }
    }



    public void InitializePlayer(SpawnManager spawnManager, UIManager uIManager)
    {
        _spawnManager = spawnManager;
        _uiManager = uIManager;
    }

    private void CalculateMovement()
    {

        //determine the move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //do the move
        transform.Translate(direction * _currentMoveSpeed * Time.deltaTime);

        //clamp y movement
        transform.position = new Vector3( transform.position.x, Mathf.Clamp(transform.position.y, _yPositionMin, _yPositionMax));

        //wrap x movement
        if (transform.position.x >= _xPositionMax)
        {
            transform.position = new Vector3(_xPositionMin, transform.position.y);
        }
        else if (transform.position.x <= _xPositionMin)
        {
            transform.position = new Vector3(_xPositionMax, transform.position.y);
        }


    }

    private void FirePlayerProjectile()
    {
        //null check
        if (_playerProjectile == null)
        {
            Debug.LogWarning("Player is trying to fire a projectile, but theres no prefab. Do you need a break?");
            return;
        }

        //assign current time + delay
        _canFire = Time.time + _fireRate;

        GameObject playerProjectile = Instantiate(_playerProjectile, transform.position + _projectileStartOffset, Quaternion.identity);

        //do sound stuff
        PlayAudio(_laserNoises);

    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.Log("No clip got assigned to player audio source");
            return;
        }
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void TakeDamage(int damage)
    {
        //handle shield
        if (_isShielded)
        {
            _isShielded = false;
            _shieldSprite.SetActive(false);
            return;
        }

        _uiManager.ChangeLivesSprite(_playerHealth);

        //handle health
        _playerHealth = _playerHealth-damage;


        if ( _playerHealth <= 0 )
        {
            PlayerDeath();
            return;
            
        }

        //handle engine fire animation
        int randomEngin = Random.Range(0, 1);
        if (_engineFireEnabled)
        {
            randomEngin = _secondEngineFire;
        }
        if (randomEngin == 0)
        {
                _leftEngine.SetActive(true);
                _secondEngineFire = 1;
                _engineFireEnabled = true;
        }
        else if (randomEngin == 1)
        {
            _rightEngine.SetActive(true);
            _secondEngineFire = 0;
            _engineFireEnabled = true;
        }
        

        //handle progressive powerups
        if (_currentPower > 0 ) 
        {
            _currentPower--;
            _playerProjectile = _projectileLists[_currentPower];
        }

        //handle speed down
        if(_currentMoveSpeed == _boostedSpeed)
        {
            _currentMoveSpeed = _normalMoveSpeed;
        }

    }

    private void PlayerDeath()
    {
        PlayAudio(_explosionNoises);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        _spawnManager.StopSpawningDoods();
        _uiManager.DisplayGameOverScreen();
        Destroy(this.gameObject);
    }

    private void CollectedPowerUp(int whichPowerUp)
    {
        
        switch (whichPowerUp)
        {
            case TRIPLE_LASER:
                LaserPowerUp();
                break;
            case SPEED_UP:
                SpeedUp();
                break;
            case SHIELD_UP:
                ShieldUp();
                break;
            default: break;
        }
        PlayAudio(_pickupNoise);
    }

    private void LaserPowerUp()
    {
        if (_currentPower == _projectileLists.Count)
        {
            return;
        }

        _currentPower++;
        _playerProjectile = _projectileLists[_currentPower];
    }
    private void SpeedUp()
    {
        _currentMoveSpeed = _boostedSpeed;
    }
    private void ShieldUp()
    {
        _isShielded = true;
        _shieldSprite.SetActive(true);
    }
}

