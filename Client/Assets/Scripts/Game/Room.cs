using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomScriptableObject _roomData;

    private void SpawnEnemies()
    {
        for(int i = 0; i < _roomData._enemySpawnsInfo._enemyCount; i++) //for each enemy in enemyCount
        {
            //take room NavMeshData
            //spawn emenies into the room for amount enemyCount inside rooms NavMesh
        }
    }
}
