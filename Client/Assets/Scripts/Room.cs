using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    #region NetworkVariables
    public static Dictionary<ushort, Room> list = new Dictionary<ushort, Room>();
    public static Dictionary<ushort, Transform> enemylist = new Dictionary<ushort, Transform>();
    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

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
        list.Remove(EnemyId);
    }
 
    public static void Spawn(string message)
    {
        //Our ID is not incrementing correctly.
        Debug.Log(message);
        string[] messageSplit = message.Split("|");
        Debug.Log(messageSplit[0]+messageSplit[1]);
        ushort tempid = Convert.ToUInt16(messageSplit[0]);
        Debug.Log(tempid);
        messageSplit[1] = messageSplit[1].Trim('(',')');
        string[] temppos = messageSplit[1].Split(",");
        Debug.Log(temppos[0] + temppos[1] + temppos[2]);
        Vector3 position = new Vector3(float.Parse(temppos[0]), float.Parse(temppos[1]), float.Parse(temppos[2]));
        Debug.Log(position);
        // var arr = message.Split(["|"], StringSplitOptions.None);
        EnemyHandler enemy = Instantiate (GameLogic.GameLogicInstance.EnemyPrefab, position, Quaternion.identity).GetComponent<EnemyHandler>();
        enemy.EnemyId = tempid;
        Debug.Log("room.EnemyId"+enemy.EnemyId);
       // enemy.EnemyPosition = position;
        enemylist.Add(tempid, enemy.transform);
    }
   
    [MessageHandler((ushort)ServerToClientId.enemySpawned)]
    private static void SpawnEnemy(Message message)
    {
        Spawn(message.GetString());
    }

    public void EnemyDied(ushort enemyID)
    {
        Debug.Log("Attempting to destroy Enemy");
        Destroy(gameObject);
        GameManager.AddScore();//THIS IS THE SCORE (CLIENT SIDED)
    }


    [MessageHandler((ushort)ServerToClientId.enemyDeath)]
    private static void EnemyDead(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Room room))
        {
            room.EnemyDied(message.GetUShort());
            // Debug.Log("Recieved enemymovement");

        }
    }



    #endregion
}
