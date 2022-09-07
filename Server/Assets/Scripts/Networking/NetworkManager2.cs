//using UnityEngine;
//using RiptideNetworking;        // using the package that allows us to network
//using RiptideNetworking.Utils;

//public enum ServerToClientId : ushort 
//{ playerSpawned = 1, }

//// converting the enum to ushort (Int16) because less memory usage
//// the flip side is it takes more processing power to convert enums
//// so decide whether it's worth converting the type due to the sheer number
//// of values within the enum, or keep it as default (Int32) due to low amount
//// of values (and therefore more processing than it's worth converting)
//// To clarify, this conversion for this project is not worth the processing power
//public enum ClientToServerId : ushort
//{ name = 1, }

//public class NetworkManager : MonoBehaviour
//{
//    /*
//     * We want to make sure there is only ONE instance of our network manager.
//     * We are creating a private static instance of our NetworkManager and a public
//     * static Property to control the instance.
//     */
//    private static NetworkManager _networkManagerInstance;
//    public static NetworkManager NetworkManagerInstance
//    {
//        // Property Read is public by default and reads the instance
//        get => _networkManagerInstance;
//        private set
//        {
//            // Property private write sets instance to the value if the
//            // instance is null, since we don't want our only instance to be null
//            if (_networkManagerInstance == null)
//            {
//                _networkManagerInstance = value;
//            }
//            // if the value of the current passed instance of NetworkManager does not have the correct value
//            // (from this class) we need to destroy that instance as there should only be one.
//            else if (_networkManagerInstance != value)
//            {
//                Debug.LogWarning($"{nameof(NetworkManager)} instance already exists, destroy duplicate!");
//                Destroy(value);
//            }
//        }
//    }
//    public Server GameServer { get; private set; }
//    [SerializeField] private ushort s_port;
//    [SerializeField] private ushort s_maxClientCount;

//    // awake happens as soon as the object is active regardless of the script being active
//    // (before the first frame)
//    private void Awake()
//    {
//        // when the object that this script is on is active in the game
//        // set the instance to this... and check to see if the instance is already set
//        NetworkManagerInstance = this;
//    }

//    // start happens when the script and object are active in the scene, on the first frame
//    private void Start()
//    {
//        // setting our game server to try to run at 60fps and not produce way more
//        // frames than necessary (when first set up was running at ~275fps)
//        Application.targetFrameRate = 60;

//        // Logs what the network is doing
//        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

//        // Create a new server
//        GameServer = new Server();
//        // starts the server at port XXXX with X amount of clients
//        GameServer.Start(s_port, s_maxClientCount);

//        // when a client leaves the server run the PlayerLeft function
//        GameServer.ClientDisconnected += PlayerLeft;
//    }

//    // checking server activity at set intervals
//    private void FixedUpdate() // executes consistently at every 0.02 seconds
//    {
//        GameServer.Tick();
//    }

//    // when the game closes it kills the connection to the server
//    private void OnApplicationQuit()
//    {
//        GameServer.Stop();
//    }

//    private void PlayerLeft(object sender_p, ClientDisconnectedEventArgs e)
//    {
//        // when a player leaves the server Destroy the player object and remove
//        // from the list
//        Destroy(Player.list[e.Id].gameObject);
//    }
//}
