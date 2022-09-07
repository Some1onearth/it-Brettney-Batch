using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player : MonoBehaviour
{
    #region Variables
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
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
            otherPlayer.SendSpawned(id);




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

    private void SendSpawned()
    {
        // Message message = Message.Create(MessageSendMode.reliable, (ushort)ServerToClientID.playerSpawned);

        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)));
    }


    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.NetworkManagerInstance.GameServer.Send(AddSpawnData(Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned)), toClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        return message;
    }


    [MessageHandler((ushort)ClientToServerId.name)]



    //  [MessageHandler((ushort)ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }


    [MessageHandler((ushort)ClientToServerId.input)]
    private static void Input(ushort fromClientId, Message message)
    {
        if (list.TryGetValue(fromClientId, out Player player))
        {
            player.Movement.SetInput(message.GetBools(6), message.GetVector3());
        }
    }




}



