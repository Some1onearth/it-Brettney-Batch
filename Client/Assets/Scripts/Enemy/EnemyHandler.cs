using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public static Dictionary<ushort, Transform> enemylist = new Dictionary<ushort, Transform>();

    public ushort EnemyId { get; set; }

    private ushort internalID;


    private void Start()
    {
        internalID = EnemyId;
    }




    //public void Move(string message)
    //{

    //    for (int i = 0; i < enemylist.Count; i++)
    //    {
    //        enemylist[id].transform.position = newPosition;
    //        enemylist[id].transform.forward = forward;
    //    }
    //    Debug.Log(EnemyId);
       
    //}



    //[MessageHandler((ushort)ServerToClientId.enemyMovement)]
    //public static void EnemyMovement(Message message)
    //{
    //    if (list.TryGetValue(message.GetString(), out EnemyHandler enemyHandler))
    //    {
    //        enemyHandler.Move(message.GetString());
    //    }
    //}


}
