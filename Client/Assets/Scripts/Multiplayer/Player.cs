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


    private void Move(ushort tick, Vector3 newPosition, Vector3 forward)
    {
       // interpolator.NewUpdate(tick, newPosition);
        transform.position = newPosition;


        model.transform.forward = forward;

        animationManager.AnimatedBasedOnSpeed();

    }

        public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.NetworkManagerInstance.GameClient.Id)
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
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
        {
            player.Move(message.GetUShort(), message.GetVector3(), message.GetVector3());
        }
    }

    

    //private void OnCollisionEnter(Collision collider)
    //{
        

    //        Debug.Log("Collission with something>");
    //        if (collider.collider.CompareTag("Enemy"))//We will send a message to the client containing what enemy was hit, and the new player Score.
    //        {
    //            Debug.Log("Collission with Enemy");


    //            Destroy(collider.gameObject);

    //        }
       
    //}


}
