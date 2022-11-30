using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public GameObject enemyMob;
    public int enemySpawnCount = 0;


    public void HowManyToSpawn(int spawnvalue)
    {
        enemySpawnCount = spawnvalue;

        for (int i = 0; i <= enemySpawnCount; i++)
        {
            SpawnEnemy();
        }


    }

    public void SpawnEnemy()
    {
        Instantiate(enemyMob);
    }







}
