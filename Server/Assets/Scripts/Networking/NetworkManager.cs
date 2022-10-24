using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;


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
    We want to make sure there is only one instance of this
    We are creating a private static instance of our NetworkManager
    and a public static Property to control the instance.
     */
    #region Variables
    private static NetworkManager _networkManagerInstance;

    #endregion
    #region Properties
    public static NetworkManager NetworkManagerInstance
    {
        //Property Read is public by default and readys the instance
        get => _networkManagerInstance;
        private set
        {
            //Property private write sets the instance to the value if the instance is null
            if (_networkManagerInstance == null)
            {
                _networkManagerInstance = value;
            }
            //Property checks for already existing NetworkManagers and if the instance doesn't match it destroys it :)
            else if (_networkManagerInstance != value)
            {
                Debug.LogWarning($"{nameof(NetworkManager)} instance already exists, destroy duplicate!");
                Destroy(value);
            }
        }
    }
    public Server GameServer { get; private set; }
    public ushort CurrentTick { get; private set; } = 0;
    [SerializeField] private ushort s_port;
    [SerializeField] private ushort s_maxClientCount;


    #endregion

    private void Awake()
    {
        //When the object that this script is attached to is activated in the game, set the instance to this and check to see if instance is already set
        NetworkManagerInstance = this;
    }

    private void Start()
    {
        //  Application.targetFrameRate = 60;
        //Logs what the network is doing
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        //Create new server
        GameServer = new Server();
        //Starts the Server at port XXXX with X amount of clients
        GameServer.Start(s_port, s_maxClientCount);

        //When a client leaves the server run the PlayerLeft function
        GameServer.ClientDisconnected += PlayerLeft;
    }

    //Checking server activity at set intervals
    private void FixedUpdate()
    {
        GameServer.Tick();
        if (CurrentTick % 200 == 0)
        {
            SendSync();
        }

        CurrentTick++;
    }

    //When the game Closes it kills the connection to the server
    private void OnApplicationQuit()
    {
        GameServer.Stop();
    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.list.TryGetValue(e.Id, out Player player))
        {
            //When a player leaves the server Destroy the player object
            Destroy(player.gameObject);
        }
    }

    private void SendSync()
    {
        Message message = Message.Create(MessageSendMode.unreliable, (ushort)ServerToClientId.sync);
        message.Add(CurrentTick);
        GameServer.SendToAll(message);
    }
}
