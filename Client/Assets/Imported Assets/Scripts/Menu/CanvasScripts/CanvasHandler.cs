using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace CanvasExample //if the name of this script was the same as "Main Menu", you can place this namespace to separate the two of the same name but different family.
{
    public class CanvasHandler : MonoBehaviour
    {
        #region Audio
        public AudioMixer masterAudio;
        public AudioSource _as;
        public AudioClip[] audioClipArray;
        public void Awake()
        {
            _as = GetComponent<AudioSource>();
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _as.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
                _as.PlayOneShot(_as.clip);
            }
        }
        public void ChangeMasterVolume(float volume)
        {
            masterAudio.SetFloat("VolumeMaster", volume);
        }
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

        public void ChangeMusicVolume(float volume)
        {
            masterAudio.SetFloat("VolumeMusic", volume);
        }
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
        public void ChangeSFXVolume(float volume)
        {
            masterAudio.SetFloat("VolumeSFX", volume);
        }
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
        public void Quality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
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
        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
        public Resolution[] resolutions;
        public Dropdown resDropDown;

        private void Start()//sets up the dropdown to display the computer's resolution
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
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }

        #region Cursor
        public Texture2D[] cursor;
        public void ChangeCursor(int selectedCursor)
        {
            Cursor.SetCursor(cursor[selectedCursor], Vector2.zero, CursorMode.Auto);
        }
        #endregion
    }
}