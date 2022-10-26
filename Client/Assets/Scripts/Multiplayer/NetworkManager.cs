using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using System;

public enum ServerToClientId : ushort
{
    sync = 1,
    playerSpawned,
    playerMovement,
}


public enum ClientToServerId : ushort  //Puts the enum outside of the class, 
{
    //sets the value to 1, the default is 0
    name = 1,
    input,
}

public class NetworkManager : MonoBehaviour
{
    /* 
    We want to make sure there can be only ONE! instance of our network manager. 
    We are creating a private static instance of our NetworkManager and a public static Property to control the instance

     */
    private static NetworkManager _networkManagerInstance;



    #region Properties
    public static NetworkManager NetworkManagerInstance //This static reference, determines the properties of the NetworkManager
    {
        //public get means you can read and get from anywhere
        //private means this can only set from here, no where else
        //TLDR; This script is READONLY when other scripts try to utilize it.

        //Property Read is public by default and reads the instance
        get => _networkManagerInstance;

        private set
        {
            //Property private write sets instance to the value if the instance is null
            if (_networkManagerInstance == null)
            {
                //if there is no other instance, then set this as the instance we wil use
                _networkManagerInstance = value;
            }
            else if (_networkManagerInstance != value)
            {
                //the $ in the string is for formatting
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroy duplicate! THERE CAN ONLY BE ONE!");
                //Destroys the script if this is not the initial instance
                Destroy(value);
            }
        }
    }

    public Client GameClient { get; private set; }


    private ushort _serverTick;
    public ushort ServerTick
    {
        get => _serverTick;

        private set
        {
            _serverTick = value;
            InterpolationTick = (ushort)(value - TicksBetweenPositionUpdates);
        }
    }

    public ushort InterpolationTick { get; private set; }
    private ushort _ticksBetweenPositionUpdates = 2;
    public ushort TicksBetweenPositionUpdates
    {
        get => _ticksBetweenPositionUpdates;
        private set
        {
            _ticksBetweenPositionUpdates = value;
            InterpolationTick = (ushort)(ServerTick - value);
        }
    }
    [SerializeField] private string s_ip;
    [SerializeField] private ushort s_port;

    [Space(10)]
    [SerializeField] private ushort tickDivergenceTolerance = 1;


    #endregion
    //ushort is an unsigned short int (16bit int)
    //for servers we ensure we can not go out of range of the protocolls
    //s_port references the server port


    private void Awake()
    {
        //When the object that this script is attachd to is active in the game, set the instance to this... an check to see if instance is already set
        NetworkManagerInstance = this;
    }

    private void Start()
    {
        //Log what the network is doing
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false); //This is inbuilt and will automatically check things, the false is for timestamps but we will need to format the time in order to use it.
        //Create a new Client, 
        GameClient = new Client();

        //   GameClient.Connect($"{s_ip}:{s_port}");
        //Connect to server
        GameClient.Connected += DidConnect;
        //Connect fail
        GameClient.ConnectionFailed += FailedToConnct;

        GameClient.ClientDisconnected += PlayerLeft;

        //Discconect
        GameClient.Disconnected += DidDisconnect;

        ServerTick = 2;
    }





    #region Events
    void DidConnect(object sender, EventArgs e)
    {

        UIManager.UIManagerInstance.SendName();
    }

    private void FailedToConnct(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
        {
            //When a player leaves the server Destroy the player object
            Destroy(player.gameObject);
        }

    }

    void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.UIManagerInstance.BackToMain();
        foreach (Player player in Player.list.Values)
        {
            Destroy(player.gameObject);
        }
    }



    #endregion




    #region Function

    private void FixedUpdate() //This allos the client and server to communicate at fixed intervals
    {
        GameClient.Tick();
        ServerTick++;
    }

    private void OnApplicationQuit() //Quits the client from the server if they quit
    {
        GameClient.Disconnect();
    }

    public void Connect()
    {
        Debug.Log("Connection attempted.");
        //Connect to Server
        GameClient.Connect($"{s_ip}:{s_port}");
    }
    #endregion




    private void SetTick(ushort serverTick)
    {
        if (Mathf.Abs(ServerTick - serverTick) > tickDivergenceTolerance)
        {
            Debug.Log($"Client Tick: {ServerTick} -> {serverTick}");
            ServerTick = serverTick;
        }
    }


    [MessageHandler((ushort)ServerToClientId.sync)]
    public static void Sync(Message message)
    {
        NetworkManagerInstance.SetTick(message.GetUShort());
    }
}
