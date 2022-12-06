using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timer;
    [SerializeField] private float _spawnDelay = 2f;


    #region Network Spawn Variables
    public static Dictionary<ushort, Room> list = new Dictionary<ushort, Room>();

    public ushort EnemyId { get; private set; }
    public string EnemyName { get; private set; }
    #endregion
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
            Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
        }

        _shouldSpawn = false;
    }


    #region Network Spawn Methods


    #endregion
}