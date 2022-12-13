    using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
     [SerializeField] private GameObject model; //

    public ushort Id { get; private set; }

    public bool IsLocal { get; private set; }

  //  [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform camTransform;
   // [SerializeField] private Interpolator interpolator;

    private string username;
    #endregion





    private void OnDestroy()
    {
        list.Remove(Id); //removes id after player is destroyed in scene.
    }


    private void Move(Vector3 newPosition, Vector3 forward, Vector3 modelfoward)//runs function after receiving message from server and places the position.
    {
       // interpolator.NewUpdate(tick, newPosition);
        transform.position = newPosition;


        if (!IsLocal)
        {
            camTransform.forward = forward;
           // playerController.AnimatedBasedOnSpeed();
        }
          
        //Debug.Log("Move Method" + forward);
        //if (!IsLocal)
        //{
        //    camTransform.forward = forward;
        //}
       // transform.forward = forward;
      //  model.transform.rotation = Quaternion.LookRotation(newPosition);
        model.transform.forward = modelfoward;

    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.NetworkManagerInstance.GameClient.Id)//Used to determine if the player is Local or not local(Can the client control this player by adding inputs)
        {
            Debug.Log("Local Player");
            player = Instantiate(GameLogic.GameLogicInstance.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }

        else
        {
            Debug.Log("Non Local Player");
            player = Instantiate(GameLogic.GameLogicInstance.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }

        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.username = username;

        list.Add(id, player);


    }

    [MessageHandler((ushort)ServerToClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)//recieves playerspawned message from server
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message)//Recieves message from server from playerMovement Message.
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetVector3(), message.GetVector3(), message.GetVector3());//Runs move function on the player with correct ushort
        }
    }





    #region PlayerControllerComments


    //This section is added as if added to player controller now it will cause conflict issues as other changes were made but have not been pulled
    //private void SendInput()//Sends the message to client with the array of bools values and the camera's forward vector to the server 
    //{
    //    Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
    //    message.AddBools(inputs, false);
    //    message.AddVector3(camTransform.forward);
    //    NetworkManager.NetworkManagerInstance.GameClient.Send(message); //Sends the message to the server with the message.
    //}

    #endregion

}
