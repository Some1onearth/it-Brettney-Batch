using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelData : MonoBehaviour
{

    public float playerSpeed = 0;
    public int playerScore = 0;
    public int playerMaxHP = 0;
    public int playerCurHP = 0;


    private void Awake()
    {

        playerScore = 0;
        playerCurHP = playerMaxHP;
       // playerSpeed = 
    }

    public void EnemyKilled()
    {
        playerSpeed += 0.25f;
        playerScore += 1;
    }



}
