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
    private struct EnemySpawnInfo
    {
        [SerializeField] [Range(1,5)] private int _enemyCount;
        [SerializeField] private GameObject _enemyPrefab;
    }

    [Header("Enemy Room Data")]
    [SerializeField] private List<EnemySpawnInfo> _enemySpawnsInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
