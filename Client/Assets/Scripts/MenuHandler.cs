using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void ChangeSceneViaIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ChangeSceneViaName(string sceneName) // This works the same as above but with names instead of scene numbers (eg. 1)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ExitTheGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //don't need UnityEditor at the beginning since UnityEditor is here. Easier to put up top 
#endif
        Application.Quit();
    }
}
