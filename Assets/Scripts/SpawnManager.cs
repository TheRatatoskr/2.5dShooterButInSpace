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

    
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _powerUpContainer;

    [SerializeField] private GameObject _uiManager;
    private UIManager _ui;

    private int _currentScore = 0;

    private bool _allowRestart = false;

    [SerializeField] GameObject _startingAsteroid;
    private void Start()
    {
        //spawn the UIManager
        GameObject uiManager = Instantiate(_uiManager, transform.position, Quaternion.identity);
        _ui = uiManager.GetComponent<UIManager>();

        //spawn the player
        GameObject player = Instantiate(_player, transform.position, Quaternion.identity);
        player.gameObject.GetComponent<Player>().InitializePlayer(this, uiManager.GetComponent<UIManager>());

        //spawn the player
        GameObject startingAsteroid = Instantiate(_startingAsteroid, transform.position, Quaternion.identity);
        startingAsteroid.gameObject.GetComponent<Asteroid>().InitializeAsteroid(this);



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
            GameObject newEnemy = Instantiate(_enemyList[Random.Range(0, _enemyList.Count)]
                ,new Vector3(Random.Range(_spawnLocationXLeft, _spawnLocationXRight), _spawnLocationY, 0),
                Quaternion.identity);
            

            newEnemy.transform.parent = _enemyContainer.transform;
            newEnemy.GetComponent<IEnemy>().InitializeEnemy(this);

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
        _ui.ChangePointText(_currentScore);
    }
}
