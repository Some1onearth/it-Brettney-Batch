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
    public static int score;
    [SerializeField] private GameObject[] roomDoor;
    [SerializeField] private Button readyButton;
    [SerializeField] private Text curMobText;
    [SerializeField] private Text curLevelText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text currencyText;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject gameHud;

    public GameObject enemyMob;
    public int currentScore;
    public int addScore = 1;
    public int currentCurrency;
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
        Currency(); // updates currency constantly.
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

        //_curLevel++;
        //_maxMobCount += _curLevel;
        //_curMobCount = _maxMobCount;
        //HowManyToSpawn(_maxMobCount);
       
    }
    private void PostGameScreen()
    {
        gameHud.SetActive(false);
        endGameScreen.SetActive(true);

    }

    public static void AddScore() // reference this within hitcollision code when adding score
    {
        score += 1;
    }

    public void Currency()
    {
        currentCurrency = currentScore * 10;
        currencyText.text = "" + currentCurrency;
        PlayerPrefs.SetInt("Currency", currentCurrency);
    }

    public void Spend(int price)
    {
        currentCurrency -= price;
        currencyText.text = "" + currentCurrency;
        PlayerPrefs.SetInt("Currency", currentCurrency);
    }

    public void HowManyToSpawn(int spawnvalue)
    {
       

        for (int i = 0; i <= spawnvalue; i++)
        {
            SpawnEnemy();
        }

        spawnvalue = 0;

    }

    public void SpawnEnemy()
    {
        Instantiate(enemyMob, new Vector3((Random.Range(-21.75f,23.25f)),0 ,(Random.Range(13,60)) ),Quaternion.identity);
    }






    /* Raw Data
     * X Min = -21.75
     * X Max = 23.25
     * Z Min = 13
     * Z Max = 60
     * 
     * 
     * 
     * 
     * 
     */

}
