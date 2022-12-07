using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    #region Network Spawn Variables
    public static Dictionary<ushort, EnemyHandler> list = new Dictionary<ushort, EnemyHandler>(); //creates lsit of all the enemies

    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    public Enemy EnemyValue => enemy;

    [SerializeField] private Enemy enemy;

    #endregion



    #region Network Enemy Spawn Methods
    private void OnDestroy()
    {
        list.Remove(EnemyId); //When an enemy is destroyed, Delete it from the list.
    }

    public static void SpawnEnemy(ushort enemyid, Vector3 enemypos)
    {
        foreach (EnemyHandler otherenemy in list.Values) //
            otherenemy.SendSpawned(enemyid);

        Vector3 _position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        //Instantiates EnemyPrefab
        EnemyHandler enemy = Instantiate(GameLogic.GameLogicInstance.EnemyPrefab, new Vector3(_position.x, _position.y, _position.z), Quaternion.identity).GetComponent<EnemyHandler>();
        //Sets Enemy ID
        enemy.EnemyId = enemyid;
        enemy.EnemyPosition = _position;



        enemy.SendSpawned();
        list.Add(enemyid, enemy);
    }

    private void SendSpawned()
    {
        // Message message = Message.Create(MessageSendMode.reliable, (ushort)ServerToClientID.playerSpawned);

        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.enemySpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.NetworkManagerInstance.GameServer.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.enemySpawned)), toClientId);
    }



    private Message AddSpawnData(Message message)
    {
        message.AddUShort(EnemyId);
        //  message.AddString(EnemyName);
        message.AddVector3(transform.position);
        return message;
    }


    [MessageHandler((ushort)ClientToServerId.enemyID)]



    //  [MessageHandler((ushort)ClientToServerId.name)]
    private static void Position(ushort fromClientId, Message message)
    {
        SpawnEnemy(fromClientId, message.GetVector3());
    }


    // [MessageHandler((ushort)ClientToServerId.enemyname)]
    // private static void Input(ushort fromClientId, Message message)
    // {
    //     //if (list.TryGetValue(fromClientId, out Room enemy))
    //     //{
    //       ////  enemy.EnemyValue.( message.GetVector3());
    //     //}
    // }
    //


    #endregion



}
