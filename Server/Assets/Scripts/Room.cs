using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    #region Network Spawn Variables
    public static Dictionary<ushort, EnemyMovement> list = new Dictionary<ushort, EnemyMovement>(); //creates lsit of all the enemies

    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    public EnemyMovement EnemyValue => room;
    public PlayerMovement Movement => movement;

    [SerializeField] private EnemyMovement room;
    [SerializeField] private PlayerMovement movement;


    #endregion

    //    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] public static ushort enemyReferenceID = 0;

    [SerializeField] public Vector3 enemySpawnPosition; //This determines where the enemy will spawn! In the start function you can determine the value.

    private void Start()
    {
        enemySpawnPosition = new Vector3(UnityEngine.Random.Range(-15, 15), 0, UnityEngine.Random.Range(20, 45));
        enemyReferenceID = 0;
        //  SpawnEnemies();
    }

    private void Update()
    {
        if (_shouldSpawn)
        {
            _shouldSpawn = false;
            SpawnEnemies();
            _shouldSpawn = false;
        }
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            enemySpawnPosition = new Vector3(UnityEngine.Random.Range(-15, 15), 0, UnityEngine.Random.Range(20, 45));
            Spawn(enemySpawnPosition);
        }
        _shouldSpawn = false;
    }

    #region Network Enemy Spawn Methods
    private void OnDestroy()
    {
        list.Remove(EnemyId); //When an enemy is destroyed, Delete it from the list.
    }

    public void Spawn(Vector3 position)
    {
        //Need to set the ID on the enemy as soon as it spawns in.
        //foreach (Room otherPlayer in list.Values)
        //    otherPlayer.SendSpawned(EnemyId);
        EnemyMovement enemy = Instantiate(_enemyPrefab, new Vector3(position.x, 1, position.z), Quaternion.identity).GetComponent<EnemyMovement>();
        enemy.EnemyId = enemyReferenceID;
        // enemy.EnemyPosition = position;
        Debug.Log(enemy.EnemyId);
        SendSpawned();
        list.Add(enemyReferenceID, enemy);
        int temp = Convert.ToInt32( enemyReferenceID);
        temp += 1;
        enemyReferenceID = Convert.ToUInt16(temp);
    }
    private void SendSpawned()
    {
        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.enemySpawned)));
    }
    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.NetworkManagerInstance.GameServer.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.enemySpawned)), toClientId);
    }
    private Message AddSpawnData(Message message)
    {
        message.AddString(enemyReferenceID+"|"+transform.position);
        return message;
    }
    #endregion
}