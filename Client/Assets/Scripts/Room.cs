using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    #region NetworkVariables
    public static Dictionary<ushort, Transform> enemylist = new Dictionary<ushort, Transform>(); //Dictionary List of the Spawned Enemies
    public static Dictionary<ushort, EnemyHandler> movementlist = new Dictionary<ushort, EnemyHandler>();//Dictioanry List of the Movement ID
    public ushort EnemyId { get; private set; } //Public get but a private set(Only this script sets the enemyID but other scripts and get the data

    #endregion
    [SerializeField] private static GameObject _enemyPrefab;
    public GameObject _prefabEnemy;
    private void Start()
    {
        _enemyPrefab = _prefabEnemy;
    }
    #region Network Methods
    private void OnDestroy()
    {
        enemylist.Remove(EnemyId);
        movementlist.Remove(EnemyId);
    }

    public static void Spawn(string message)
    {
        //Splits the recieved message into tangible data, Ushort ID &  Vector3 Transform
        string[] messageSplit = message.Split("|");
        ushort tempid = Convert.ToUInt16(messageSplit[0]);
        messageSplit[1] = messageSplit[1].Trim('(',')');
        string[] temppos = messageSplit[1].Split(",");
        Vector3 position = new Vector3(float.Parse(temppos[0]), float.Parse(temppos[1]), float.Parse(temppos[2]));
        
        
        EnemyHandler enemy = Instantiate (GameLogic.GameLogicInstance.EnemyPrefab, position, Quaternion.identity).GetComponent<EnemyHandler>();//Instantaites the prefab and gets the specifc enemyhandler on the spawned object
        enemy.EnemyId = tempid;//Assigned the ID of the enemy
        Debug.Log("room.EnemyId"+enemy.EnemyId);
        enemylist.Add(tempid, enemy.transform);//Adds to the list
        movementlist.Add(tempid, enemy);//adds to the list

    }

    [MessageHandler((ushort)ServerToClientId.enemySpawned)]
    private static void SpawnEnemy(Message message)//Recieves a message from server (enemySpawned message) as a string
    {
        Spawn(message.GetString());
    }

   


    #endregion
}
