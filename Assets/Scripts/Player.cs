using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

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
    private const int AMMO_UP = 31;
    private const int HEALTH_UP = 41;
    private int _currentPower = 1;

    [Tooltip("How fast the player can shoot")]
    [SerializeField] private float _fireRate = .5f;
    private float _canFire =-1f;

    [Header("Stats")]
    [SerializeField] private int _playerHealth = 3;
    [SerializeField] private int _maxPlayerHealth = 3;

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

    [Header("Run Button")]
    [SerializeField] private float _runMultiplier = 2f;
    [SerializeField] private float _baseMultiplier = 1f;
    [SerializeField] private float _maxBoostTime;
    private float _currentBoostTime;
    private float _currentMultiplier;
    private bool _canBoost = true;

    [Header("Shield Management")]
    [SerializeField] private List<Color> _shieldStrengthColor;
    [SerializeField] private GameObject _shieldObject;
    private int _shieldLife = 2;
    private bool _isShielded = false;
    private SpriteRenderer _shieldSprite;

    [Header("Ammo Stuff")]
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _minStartingAmmo;
    private int _currentAmmo;

    [SerializeField] private int _minAmmoUp;
    [SerializeField] private int _maxAmmoUp;

    [Header("Camera Shake")]
    [SerializeField] private float _shakeStrength;
    [SerializeField] private float _shakeLength;
    private Vector3 _cameraStartPosition;
    private GameObject _theCamera;

    public void InitializePlayer(SpawnManager spawnManager, UIManager uIManager)
    {
        _spawnManager = spawnManager;
        _uiManager = uIManager;
        
    }

    private void Start()
    {
        _theCamera = Camera.main.gameObject;
        _cameraStartPosition = new Vector3(_theCamera.transform.position.x, _theCamera.transform.position.y, _theCamera.transform.position.z);

        _currentAmmo = Random.Range(_minStartingAmmo, _maxAmmo);
        PlayerAmmoChange();

        _currentMultiplier = _baseMultiplier;

        transform.position = _startingPosition;

        _shieldObject.gameObject.SetActive(false);
        _shieldSprite = _shieldObject.GetComponent<SpriteRenderer>();

        _audioSource = GetComponent<AudioSource>();

    }

    private void PlayerAmmoChange()
    {
        _uiManager.ChangeAmmoCounter($"{_currentAmmo.ToString()}/{_maxAmmo.ToString()}");
    }

    private void Update()
    {
        CalculateMovement();

        if (Input.GetButtonDown("Jump") && Time.time > _canFire)
        {
            FirePlayerProjectile();
        }

        if (Input.GetButton("Fire3"))
        {
            BoosterMode();

        }
        if (Input.GetButtonUp("Fire3"))
        {
            _canBoost = false;
            _currentMultiplier = _baseMultiplier;
        }

        if(!_canBoost)
        {
            _currentBoostTime-= Time.deltaTime;

            _uiManager.ChangeBoostMeter(_currentBoostTime / _maxBoostTime);
            if (_currentBoostTime <= 0)
            {
                _currentBoostTime = 0;
                _canBoost = true;
            }
        }

    }

    private void BoosterMode()
    {
        
        if(_currentBoostTime > _maxBoostTime)
        {
            _canBoost = false;
            _currentMultiplier = _baseMultiplier;
            return;
        }

        if (_canBoost)
        {
            _uiManager.ChangeBoostMeter(_currentBoostTime / _maxBoostTime);

            _currentBoostTime+= Time.deltaTime;

            _currentMultiplier = _runMultiplier;
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





    private void CalculateMovement()
    {

        //determine the move
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        //do the move
        transform.Translate(direction * _currentMoveSpeed * _currentMultiplier * Time.deltaTime);

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
        if (_currentAmmo <= 0 )
        {
            _currentAmmo = 0;
            return;
        }

        //assign current time + delay
        _canFire = Time.time + _fireRate;

        _currentAmmo--;
        PlayerAmmoChange();

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
            ShieldTakesTheHitInstead();

            return;
        }



        //handle health
        _playerHealth = _playerHealth-damage;

        StartCoroutine(ShakeyCameraHands());

        _uiManager.ChangeLivesSprite(_playerHealth);


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
        if (_currentPower > 1 ) 
        {
            _currentPower--;
            _playerProjectile = _projectileLists[_currentPower-1];
        }

        //handle speed down
        if(_currentMoveSpeed == _boostedSpeed)
        {
            _currentMoveSpeed = _normalMoveSpeed;
        }

    }

    private void ShieldTakesTheHitInstead()
    {

        if (_shieldLife <= 0)
        {
            _isShielded = false;
            _shieldObject.SetActive(false);
        }
        else
        {
            _shieldLife--;
            _shieldSprite.color = _shieldStrengthColor[_shieldLife];
        }


        return;
    }

    private void PlayerDeath()
    {

        Instantiate(_explosion, transform.position, Quaternion.identity);

        _spawnManager.StopSpawningDoods();

        _uiManager.DisplayGameOverScreen();

        Destroy(this.gameObject, .3f);
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
            case AMMO_UP:
                AmmoUp();
                break;
            case HEALTH_UP:
                HealthUp();
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
        _playerProjectile = _projectileLists[_currentPower-1];
    }

    private void SpeedUp()
    {
        _currentMoveSpeed = _boostedSpeed;
    }

    private void ShieldUp()
    {

        _isShielded = true;

        _shieldLife = _shieldStrengthColor.Count - 1;
        _shieldObject.SetActive(true);

        _shieldSprite.color = _shieldStrengthColor[_shieldLife];
    }

    private void AmmoUp()
    {
        
        _currentAmmo += Random.Range(_minAmmoUp, _maxAmmoUp);
        if (_currentAmmo > _maxAmmo)
        {
            _currentAmmo = _maxAmmo;
        }

        PlayerAmmoChange();
    }

    private void HealthUp()
    {

        switch(_playerHealth)
        {
            case 3:
                _playerHealth = _maxPlayerHealth;
                return;
                break;
            case 2:
                _engineFireEnabled = false;
                _rightEngine.SetActive(false);
                _leftEngine.SetActive(false);
                break;
            case 1:
                if (_secondEngineFire == 0)
                {
                    _leftEngine.SetActive(false);
                    _secondEngineFire = 0;
                }
                else
                {
                    _rightEngine.SetActive(false);
                    _secondEngineFire = 1;
                }
                break;
            default:
                Debug.LogWarning("How did we get here? Something is wrong go fix it.");
                break;
        }

        _playerHealth++;

        _uiManager.ChangeLivesSprite(_playerHealth);
    }

    private IEnumerator ShakeyCameraHands()
    {
        float shakeTime = 0f;
        while (shakeTime < _shakeLength)
        {
            float xShake = Random.Range(-1f, 1f) * _shakeStrength;
            float yShake = Random.Range(-1f, 1f) * _shakeStrength;

            _theCamera.transform.position = _cameraStartPosition + new Vector3(xShake, yShake, 0);

            shakeTime += Time.deltaTime;

            yield return null;
        }
        _theCamera.transform.position = _cameraStartPosition;
    }
}

