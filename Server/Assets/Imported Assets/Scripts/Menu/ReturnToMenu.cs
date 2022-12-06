using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    private Vector2 scr;

    private void OnGUI()//runs per frame same as update...
    {
        //screen width and height is broken up into 16 by 9 sections in a grid
        scr.x = Screen.width / 16;
        scr.y = Screen.height / 9;
        if (GUI.Button(new Rect(1 * scr.x, 8 * scr.y, 2 * scr.x, 0.5f * scr.y), "Back"))
        {
            //this button allows us to start the game
            //changes scenes
            SceneManager.LoadScene(0);
        }
        
        if (GUI.Button(new Rect(14 * scr.x, 8 * scr.y, 2 * scr.x, 0.5f * scr.y), "Next"))
        {
            //this button allows us to start the game
            //changes scenes
            SceneManager.LoadScene(2);
        }
    }
}
