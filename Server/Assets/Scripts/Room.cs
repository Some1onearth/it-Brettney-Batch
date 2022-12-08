using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region Network Spawn Variables
    public static Dictionary<ushort, Room> list = new Dictionary<ushort, Room>(); //creates lsit of all the enemies
   
    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    public EnemyMovement EnemyValue => room;

    [SerializeField] private EnemyMovement room;

  
    #endregion

    //    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private ushort enemyReferenceID = 0;


    private void Awake()
    {
        
    }
    private void Start()
    {
        enemyReferenceID = 0;
        //  SpawnEnemies();
    }

    private void Update()
    {
        if (_shouldSpawn)
        {
            _shouldSpawn = false;
            Debug.Log("Enemies Should Spawn");

            SpawnEnemies();
            _shouldSpawn = false;

        }
    }


    private void SpawnEnemies()
    {
        for (int i = 0; i < _enemiesToSpawn; i++)
        {
            Vector3 _position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Spawn(_position);
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
        Room enemy = Instantiate(_enemyPrefab, new Vector3(position.x, 1, position.z), Quaternion.identity).GetComponent<Room>();
        enemy.EnemyId = enemyReferenceID;
        // enemy.EnemyPosition = position;
        enemyReferenceID++;
        enemy.SendSpawned();
        list.Add(enemyReferenceID, enemy);
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
        message.AddUShort(EnemyId);
        message.AddVector3(transform.position);
        return message;
    }
  



    #endregion



}