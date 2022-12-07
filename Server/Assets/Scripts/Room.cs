using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    

    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
  //  [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timer;
    [SerializeField] private float _spawnDelay = 2f;


    
    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (_shouldSpawn)
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnDelay)
            {
                SpawnEnemies();
            }
        }
    }

    private void SpawnEnemies()



    {
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
        //  EnemyHandler.SpawnEnemy();
         //   Vector3 _position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
         //   NetworkManager.NetworkManagerInstance.InstaniateEnemy(_position); //This spawns them from the correct place
         //  // Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
        }
        _shouldSpawn = false;
    }
  

}