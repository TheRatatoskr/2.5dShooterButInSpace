using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemyList;
    [Header("Define the location(s) to spawn enemies")]
    [Tooltip("Define how high up to spawn enemies")]
    [SerializeField] private float _spawnLocationY;
    [Tooltip("Define how far to the left to spawn enemies")]
    [SerializeField] private float _spawnLocationXLeft;
    [Tooltip("Define how far to the right to spawn enemies")]
    [SerializeField] private float _spawnLocationXRight;

    [SerializeField] private bool _isSpawning;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    [SerializeField] private GameObject _player;

    [SerializeField]
    private GameObject _enemyContainer;

    private void Start()
    {
        //spawn the player
        GameObject player = Instantiate(_player, transform.position, Quaternion.identity);
        player.gameObject.GetComponent<Player>().InitializePlayer(this);

        StartCoroutine(SpawnSomeDoods());
    }

    IEnumerator SpawnSomeDoods()
    {
        while(_isSpawning)
            {
            GameObject newEnemy = Instantiate(_enemyList[Random.Range(0, _enemyList.Count)]
                ,new Vector3(Random.Range(_spawnLocationXLeft, _spawnLocationXRight), _spawnLocationY, 0),
                Quaternion.identity);
            

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
        }
    }

    public void StopSpawningDoods()
    {
        _isSpawning = false;
    }
}
