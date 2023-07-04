using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    private void Start()
    {
        //spawn the player
        GameObject player = Instantiate(_player, transform.position, Quaternion.identity);
        player.gameObject.GetComponent<Player>().InitializePlayer(this);

        StartCoroutine(SpawnSomeDoods());
        StartCoroutine(SpawnSomePowerUp());
    }

    IEnumerator SpawnSomeDoods()
    {
        while(_isSpawning)
            {
            GameObject newEnemy = Instantiate(_enemyList[Random.Range(0, _enemyList.Count)]
                ,new Vector3(Random.Range(_spawnLocationXLeft, _spawnLocationXRight), _spawnLocationY, 0),
                Quaternion.identity);
            

            newEnemy.transform.parent = _enemyContainer.transform;

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
    }
}
