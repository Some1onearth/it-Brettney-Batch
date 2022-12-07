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

    public Enemy EnemyValue => enemy;

    [SerializeField] private Enemy enemy;

    #endregion

    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timer;
    [SerializeField] private float _spawnDelay = 2f;
    [SerializeField] private ushort enemyReferenceID = 0;


    private void Start()
    {
        enemyReferenceID = 0;
        SpawnEnemies();
    }

    private void Update()
    {
        if (_shouldSpawn)
        {
            Debug.Log("Enemies Should Spawn");
         //   _timer += Time.deltaTime;
          //  if (_timer >= _spawnDelay)
          //  {
                SpawnEnemies();
            _shouldSpawn = false;
              //  _timer = 0;
           // }
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i <= _enemiesToSpawn; i++)
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

    public void Spawn(Vector3 pos)
    {
        Room enemy = Instantiate(GameLogic.GameLogicInstance.EnemyPrefab, new Vector3(pos.x, 1, pos.z), Quaternion.identity).GetComponent<Room>();
        enemy.EnemyId = enemyReferenceID;
        enemy.EnemyPosition = pos;

        enemy.SendSpawned();
        list.Add(enemyReferenceID, enemy);
        //  Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);

    }


    #region OldCode
    //public static void SpawnEnemy(ushort enemyid, Vector3 enemypos)
    //{
    //    foreach (Room otherenemy in list.Values) //
    //        otherenemy.SendSpawned(enemyid);

    //    Vector3 _position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
    //    //Instantiates EnemyPrefab
    //    Room enemy = Instantiate(GameLogic.GameLogicInstance.EnemyPrefab, new Vector3(_position.x, _position.y, _position.z), Quaternion.identity).GetComponent<Room>();
    //    //Sets Enemy ID
    //    enemy.EnemyId = enemyid;
    //    enemy.EnemyPosition = _position;



    //    enemy.SendSpawned();//Passes on the enemy values
    //    list.Add(enemyid, enemy);
    //}
    #endregion
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


    //[MessageHandler((ushort)ClientToServerId.enemyID)]



  
    //private static void Position(ushort fromClientId, Message message)
    //{
    //    Spawn(fromClientId, message.GetVector3());
    //}




    #endregion



}