    using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject selectedSkin0, selectedSkin1, defaultSkin;
    #endregion

    public ushort Id { get; private set; }

    public bool IsLocal { get; private set; }

    [SerializeField] private PlayerAnimationManager animationManager;
    [SerializeField] private Transform camTransform;
    //  [SerializeField] private Interpolator interpolator;

    private string username;

    private void Start()
    {
        switch (SkinSelection.skinIndex) //checks skinIndex from SkinSelection class
        {
            case 0: //case 0
                //sets Skin0 active and other skins disabled
                selectedSkin0.SetActive(true);
                selectedSkin1.SetActive(false);
                defaultSkin.SetActive(false);
                break;
            case 1: //case 1
                //sets Skin1 active and other skins disabled
                selectedSkin0.SetActive(false);
                selectedSkin1.SetActive(true);
                defaultSkin.SetActive(false);
                break;
        }

    }

    private void OnDestroy()
    {
        list.Remove(Id);
    }


    private void Move(ushort tick, Vector3 newPosition, Vector3 forward)//runs function after receiving message from server and places the position.
    {
        transform.position = newPosition;


        model.transform.forward = forward;

        animationManager.AnimatedBasedOnSpeed();

    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.NetworkManagerInstance.GameClient.Id)//Used to determine if the player is Local or not local(Can the client control this player by adding inputs)
        {

            player = Instantiate(GameLogic.GameLogicInstance.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }

        else
        {
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
            player.Move(message.GetUShort(), message.GetVector3(), message.GetVector3());//Runs move function on the player with correct ushort
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
