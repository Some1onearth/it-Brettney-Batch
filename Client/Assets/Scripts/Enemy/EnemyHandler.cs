using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHandler : MonoBehaviour
{
    public ushort EnemyId { get; set; } //public get and set for EnemyID

    public void Move(Vector3 position, Vector3 forward)//Recieves the position and forward and sets the values.
    {
            transform.position = position;
            transform.forward = forward;
    }

    public void EnemyDied(ushort enemyID) //Recieves the ID from the server of which enemy it was then that enemy is destroyed.
    {
        Debug.Log("Attempting to destroy Enemy");
        Destroy(gameObject);
        GameManager.gameManager.AddScore();//THIS IS THE SCORE (CLIENT SIDED)
    }

    [MessageHandler((ushort)ServerToClientId.enemyMovement)]
    private static void EnemyMove(Message message)//Recieves the message from the server for enemyMovement
    {
        //Cuts the string that was recieved into tangible data, USHORT ID, TRANSFORM POSITION, TRANSFORM FORWARD
        string stringmessage = message.GetString().ToString();
        string[] messageSplit = stringmessage.Split("|");
        ushort id = Convert.ToUInt16(messageSplit[0]);
        messageSplit[1] = messageSplit[1].Trim('(', ')');
        string[] temppos = messageSplit[1].Split(",");
        Vector3 position = new Vector3(float.Parse(temppos[0]), float.Parse(temppos[1]), float.Parse(temppos[2]));
        messageSplit[2] = messageSplit[2].Trim('(', ')');
        string[] tempforward = messageSplit[1].Split(",");
        Vector3 forward = new Vector3(float.Parse(tempforward[0]), float.Parse(tempforward[1]), float.Parse(tempforward[2]));
        EnemyHandler enemy = Room.movementlist[id]; //Sets the ID 
        enemy.Move(position, forward);//Sends the position and forward to move function
    }

    [MessageHandler((ushort)ServerToClientId.enemyDeath)]
    private static void EnemyDead(Message message)
    {
        ushort id = message.GetUShort(); //Gets the Ushort ID from enemy Death(FROM SERVER)
        EnemyHandler enemy = Room.movementlist[id]; //Sets enemy 
        enemy.EnemyDied(id);//Tells the specific enemy with the same ID that it is dead.
        Debug.Log("EnemyDead Function Message Recieved.");
       
    }


}
