using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /*
    All players load into spawn,

    When confirmed open the door to next room,
    When all mobs defeated open next door,


    Add Score when room is cleared?
    Repeat?


     */

    [SerializeField] private GameObject[] roomDoor;
    [SerializeField] private Button readyButton;
    [SerializeField] private Text curMobText;
    [SerializeField] private Text curLevelText;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject gameHud;
    private int _maxMobCount = 0;
    private int _curMobCount = 0;
    private int _curLevel = 0;
    private bool gameStart = false;
    
    

    private void Awake()
    {
        endGameScreen.SetActive(false);
        gameHud.SetActive(true);
       // _curLevel = 1;
        _maxMobCount = 4;
        _curMobCount = 0;
        gameStart = false;
    }

    public void Update()
    {
        if (gameStart)
        {
            curMobText.text = "Current Enemies Remaining: " + _curMobCount + "/" + _maxMobCount;
        }
       

        curLevelText.text = "Stage: " + _curLevel;
    }


    public void OpenNextDoor()
    {
        if (roomDoor.Length <_curLevel+1)
        {
            Debug.Log("Finished the game");
            PostGameScreen();
            return;
        }


        roomDoor[_curLevel].SetActive(false);
        NewLevel();
        gameStart = true;
    }

    private void NewLevel()
    {
        _curLevel++;
        _maxMobCount += _curLevel;
        _curMobCount = _maxMobCount;
    }
    private void PostGameScreen()
    {
        gameHud.SetActive(false);
        endGameScreen.SetActive(true);

    }

}
