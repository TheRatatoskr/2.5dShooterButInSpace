using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemyList;
    [SerializeField] private List<GameObject> _powerUpList;

    [Header("Define the location(s) to spawn enemies")]
    [Tooltip("Define how high up to spawn entities")]
    [SerializeField] private float _spawnLocationY;
    [Tooltip("Define how far to the left to spawn entities")]
    [SerializeField] private float _spawnLocationXLeft;
    [Tooltip("Define how far to the right to spawn entities")]
    [SerializeField] private float _spawnLocationXRight;

    [SerializeField] private bool _isSpawning;

    [SerializeField] private float _minEnemySpawnTime;
    [SerializeField] private float _maxEnemySpawnTime;

    [SerializeField] private float _minPowerUpSpawnTime;
    [SerializeField] private float _maxPowerUpSpawnTime;

    [SerializeField] private GameObject _player;

    [SerializeField] private int _startShieldEnemyWave;
    [SerializeField] private int _chanceOfUnshieldedEnemy;

    [SerializeField] private GameObject _enemyContainer;

    [SerializeField] private GameObject _powerUpContainer;

    [SerializeField] private GameObject _uiManager;

    [SerializeField] private int _waveEnemyCountModifier;
    
    private List<GameObject> _currentListOfEnemies;

    private int _currentWave = 0;

    private bool _waveComplete = false;

    private UIManager _ui;

    private int _currentScore = 0;

    private bool _allowRestart = false;

    [SerializeField] private int numberOfEnemiesInWave;
    [SerializeField] GameObject _startingAsteroid;


    private void Start()
    {
        //spawn the UIManager
        GameObject uiManager = Instantiate(_uiManager, transform.position, Quaternion.identity);
        _ui = uiManager.GetComponent<UIManager>();

        //spawn the player
        GameObject player = Instantiate(_player, transform.position, Quaternion.identity);
        player.gameObject.GetComponent<Player>().InitializePlayer(this, uiManager.GetComponent<UIManager>());

        //spawn the asteroid
        GameObject startingAsteroid = Instantiate(_startingAsteroid, transform.position, Quaternion.identity);
        startingAsteroid.gameObject.GetComponent<Asteroid>().InitializeAsteroid(this);

        //Setup the first wave
        SetupAWave();



    }

    private void Update()
    {
        if (_allowRestart && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(_waveComplete)
        {
            if (_enemyContainer.transform.childCount == 0)
            {
                SetupAWave();
            }
        }
    }

    private void SetupAWave()
    {
        _currentWave++;

        _currentListOfEnemies = new List<GameObject>();
        numberOfEnemiesInWave = _currentWave * _waveEnemyCountModifier + Random.Range(1, _currentWave);

        int enemyTypeCounter = _currentWave -1;

        if (enemyTypeCounter > _enemyList.Count)
        {
            enemyTypeCounter = _enemyList.Count - 1;
        }

        for(int i = 0; i < numberOfEnemiesInWave; i++)
        {
            _currentListOfEnemies.Add(_enemyList[Random.Range(0, enemyTypeCounter)]);
        }

        _waveComplete = false;
        _isSpawning = true;

        _ui.ChangeCurrentWaveCounter(_currentWave.ToString());
        _ui.ChangeEnemyRemainingText(numberOfEnemiesInWave.ToString());

        if (_currentWave != 1)
        {
            StartSpawningDoods();
        }
    }

    public void StartSpawningDoods()
    {
        StartCoroutine(SpawnSomeDoods());
        StartCoroutine(SpawnSomePowerUp());
    }

    IEnumerator SpawnSomeDoods()
    {
        yield return new WaitForSeconds(5f);

        while(_isSpawning)
            {
            GameObject newEnemy = Instantiate(_currentListOfEnemies[0]
                ,new Vector3(Random.Range(_spawnLocationXLeft, _spawnLocationXRight), _spawnLocationY, 0),
                Quaternion.identity);
            

            newEnemy.transform.parent = _enemyContainer.transform;
            
            IEnemy enemy = newEnemy.GetComponent<IEnemy>();

            enemy.InitializeEnemy(this);

            if (_currentWave >= _startShieldEnemyWave)
            {
                int shieldCheck = Random.Range(0, 100);

                

                if (shieldCheck + _currentWave > _chanceOfUnshieldedEnemy) 
                {
                    enemy.ActivateShields();
                }

            }

            _currentListOfEnemies.RemoveAt(0);

            if(_currentListOfEnemies.Count == 0)
            {
                _isSpawning = false;
                _waveComplete = true;
            }
            yield return new WaitForSeconds(Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
        }
    }

    IEnumerator SpawnSomePowerUp()
    {

        while (_isSpawning)
        {
            GameObject newPowerUp = Instantiate(_powerUpList[Random.Range(0, _powerUpList.Count)],
                                             new Vector3(Random.Range(_spawnLocationXLeft, _spawnLocationXRight), _spawnLocationY, 0),
                                              Quaternion.identity);


            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(_minPowerUpSpawnTime, _maxPowerUpSpawnTime));
        }

    }

    public void StopSpawningDoods()
    {
        _isSpawning = false;
        _allowRestart = true;

    }

    public void AMurderWasReported()
    {
        _currentScore++;
        numberOfEnemiesInWave--;
        UpdateUIOnEnemyDeath();
    }

    public void UpdateUIOnEnemyDeath()
    {
        _ui.ChangePointText(_currentScore);
        _ui.ChangeEnemyRemainingText(numberOfEnemiesInWave.ToString());
    }
}
