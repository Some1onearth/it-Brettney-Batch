using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player : MonoBehaviour
{
    //part of send movement, make bool isAttacking, then send it back to client, make another script like this for enemy
    #region Variables
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();//creates dictionary of all the players.
    #endregion
    #region Properties
    public ushort Id { get; private set; }
    public string Username { get; private set; }

    public PlayerMovement Movement => movement;

    [SerializeField] private PlayerMovement movement;
    #endregion



    private void OnDestroy()
    {
        list.Remove(Id);
    }


    public static void Spawn(ushort id, string username) 
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id); //Ensures all other clients are spawned into the game correctly.
       



        //Instantiates PlayerPrefab
        Player player = Instantiate(GameLogic.GameLogicInstance.PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Player>();
        //Sets Player name (creates a default name if username is not filled in)
        player.name = $"Player{id}({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        //Sets Player ID
        player.Id = id;
        //Sets Player Username (creates a default username if username is not filled in)
        player.Username = string.IsNullOrEmpty(username) ? "Guest" : username;
        player.SendSpawned();
        list.Add(id, player);
        
    }

    private void SendSpawned()//Sends the message through
    {
        // Message message = Message.Create(MessageSendMode.reliable, (ushort)ServerToClientID.playerSpawned);

        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)));
    }


    private void SendSpawned(ushort toClientId)//SendSpawned Overlaod Function
    {

        NetworkManager.NetworkManagerInstance.GameServer.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)//Adds additional Spawn Data before sending back to SendSpawnedMethod.
    {
        message.AddUShort(Id);//Adds the spawn data in for sending messages purposes.
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;
    }


    [MessageHandler((ushort)ClientToServerId.name)]//REcieves the players Name from the client
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString()); 
    }


    [MessageHandler((ushort)ClientToServerId.input)]//Used to recieve the input of the client that can be used to control the player.
    private static void Input(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
        {
            player.Movement.SetInput(message.GetBools(6), message.GetVector3());
        }
    }




}



