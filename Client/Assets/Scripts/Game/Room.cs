using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region NetworkVariables
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();


    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }


    [SerializeField] private Interpolator interpolator;
    [SerializeField] private PlayerAnimationManager animationManager;
    #endregion

    [SerializeField] private int _currentWave = 0, _maxWave = 3;
    [SerializeField] private bool _shouldSpawn;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private GameObject _enemyPrefab;
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
        for(int i = 0; i < _enemiesToSpawn; i++)
        {
            Instantiate(_enemyPrefab, new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)), Quaternion.identity);
        }

        _shouldSpawn = false;
    }



    #region Network Methods
    private void OnDestroy()
    {
        list.Remove(EnemyId);
    }


    private void Move(ushort tick, Vector3 newPosition, Vector3 forward)
    {
        interpolator.NewUpdate(tick, newPosition);
        //transform.position = newPosition;


        _enemyPrefab.transform.forward = forward;

        animationManager.AnimatedBasedOnSpeed();

    }

    //public static void Spawn(ushort id, string username, Vector3 position)
    //{

    //    Player player;
    //    if (id == NetworkManager.NetworkManagerInstance.GameClient.Id)
    //    {

    //        player = Instantiate(GameLogic.GameLogicInstance.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
    //        player.IsLocal = true;
    //    }

    //    else
    //    {
    //        player = Instantiate(GameLogic.GameLogicInstance.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
    //        player.IsLocal = false;
    //    }

    //    player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
    //    player.Id = id;
    //    player.username = username;

    //    list.Add(id, player);


    //}

    //[MessageHandler((ushort)ServerToClientId.playerSpawned)]


    //private static void SpawnPlayer(Message message)
    //{
    //    Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    //}

    //[MessageHandler((ushort)ServerToClientId.playerMovement)]
    //private static void PlayerMovement(Message message)
    //{
    //    if (list.TryGetValue(message.GetUShort(), out Player player))
    //    {
    //        player.Move(message.GetUShort(), message.GetVector3(), message.GetVector3());
    //    }
    //}


    #endregion
}
