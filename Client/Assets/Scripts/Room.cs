using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    #region NetworkVariables
    public static Dictionary<ushort, Room> list = new Dictionary<ushort, Room>();
    public ushort EnemyId { get; private set; }
    public Vector3 EnemyPosition { get; private set; }

    [SerializeField] private Interpolator interpolator;
    // [SerializeField] private PlayerAnimationManager animationManager;
    #endregion
    [SerializeField] private static GameObject _enemyPrefab;
    public GameObject _prefabEnemy;
    private void Start()
    {
        _enemyPrefab = _prefabEnemy;
    }
    #region Network Methods
    private void OnDestroy()
    {
        list.Remove(EnemyId);
    }
    private void Move(ushort tick, Vector3 newPosition, Vector3 forward)
    {
        interpolator.NewUpdate(tick, newPosition);
        _enemyPrefab.transform.forward = forward;
        //  animationManager.AnimatedBasedOnSpeed();
    }
    public static void Spawn(ushort id, Vector3 position)
    {
        Room room = Instantiate(GameLogic.GameLogicInstance.EnemyPrefab, new Vector3(position.x, 1, position.z), Quaternion.identity).GetComponent<Room>();
        room.EnemyId = id;
        room.EnemyPosition = position;
        list.Add(id, room);
    }

    [MessageHandler((ushort)ServerToClientId.enemySpawned)]


    private static void SpawnEnemy(Message message)
    {
        Spawn(message.GetUShort(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToClientId.enemyMovement)]
    private static void EnemyMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Room room))
        {
            room.Move(message.GetUShort(), message.GetVector3(), message.GetVector3());
        }
    }


    #endregion
}
