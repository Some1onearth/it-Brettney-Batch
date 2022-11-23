using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room Data", menuName = "RoomData")]
public class RoomScriptableObject : ScriptableObject
{
    [System.Serializable]
    public enum RoomTypes
    {
        EnemyRoom,
        BossRoom
    }

    [SerializeField] private RoomTypes _roomType;
    
    [System.Serializable]
    public struct EnemySpawnInfo
    {
        [SerializeField] [Range(1,5)] public int _enemyCount;
        [SerializeField] public List<GameObject> _enemyPrefabs;
    }

    [Header("Enemy Room Data")]
    [SerializeField] public EnemySpawnInfo _enemySpawnsInfo;
}
