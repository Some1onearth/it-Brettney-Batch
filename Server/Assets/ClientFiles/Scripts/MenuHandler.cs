using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    #region Audio
    public AudioMixer masterAudio;
    public AudioSource _as;
    public AudioClip[] audioClipArray; //add more than one audio file here for randomisation
    public void Awake()
    {
        _as = GetComponent<AudioSource>(); //plays the audio sooner than start
    }
    /*
    public void Update()
    {
        //this chooses sound everytime you click on the mouse
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
            _as.PlayOneShot(_as.clip);
        }
    }*/

    //changes the master volume level
    public void ChangeMasterVolume(float volume) 
    {
        masterAudio.SetFloat("VolumeMaster", volume);
    }

    //allows you toggle the master volume on/off
    public void ToggleMasterMute(bool isMuted) 
    {
        if (isMuted)
        {
            masterAudio.SetFloat("VolumeMaster", -80);
        }
        else
        {
            masterAudio.SetFloat("VolumeMaster", 0);
        }
    }

    //changes the music volume level
    public void ChangeMusicVolume(float volume)
    {
        masterAudio.SetFloat("VolumeMusic", volume);
    }

    //toggle the music on/off
    public void ToggleMusicMute(bool isMuted)
    {
        if (isMuted)
        {
            masterAudio.SetFloat("VolumeMusic", -80);
        }
        else
        {
            masterAudio.SetFloat("VolumeMusic", 0);
        }
    }
    //change the SFX volume level
    public void ChangeSFXVolume(float volume)
    {
        masterAudio.SetFloat("VolumeSFX", volume);
    }

    //toggles the SFX on/off
    public void ToggleSFXMute(bool isMuted)
    {
        if (isMuted)
        {
            masterAudio.SetFloat("VolumeSFX", -80);
        }
        else
        {
            masterAudio.SetFloat("VolumeSFX", 0);
        }
    }
    #endregion

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

    #region Resolution
    //changes the quality of the graphics
    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //toggles the resolution 
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public Resolution[] resolutions;
    public Dropdown resDropDown;

    //chooses the resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    #endregion


    #region Cursor
    public Texture2D[] cursor;
    public void ChangeCursor(int selectedCursor)
    {
        Cursor.SetCursor(cursor[selectedCursor], Vector2.zero, CursorMode.Auto);
    }
    #endregion

    //sets up the dropdown to display the computer's resolution
    private void Start()
    {
        if (resDropDown != null)
        {
            resolutions = Screen.resolutions;
            resDropDown.ClearOptions();
            List<string> resOptions = new List<string>();
            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                resOptions.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resDropDown.AddOptions(resOptions);
            resDropDown.value = currentResolutionIndex;
            resDropDown.RefreshShownValue();
        }
        else
        {
            Debug.LogWarning("SCRUB ATTACH THE DROP DOWN!!!!!");
        }


    }
}
