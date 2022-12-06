using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CanvasExample
{

    public class KeyBindsManager : MonoBehaviour
    {
        [SerializeField]
        public static Dictionary<string, KeyCode> inputKeys = new Dictionary<string, KeyCode>();

        [System.Serializable]//Allows us to see our struct when we use it in an array
        public struct KeyUISetup
        {
            public string keyName;
            public Text keyDisplayText;
            public string defaultKey;
        }
        //array of our Key Names, UI Text for the Key and Default Key
        public KeyUISetup[] baseSetup;
        public GameObject currentKey;
        public Color32 changedKey = new Color32(39, 171, 249, 255);
        public Color32 selectedKey = new Color32(239, 116, 36, 255);

        // Start is called before the first frame update
        void Start()
        {
            if (inputKeys.Count <= 0) //if we have our keys then skip this step
            {
                //for loop to add the keys to the Dictionary with Save or Default depending on load
                for (int i = 0; i < baseSetup.Length; i++)//for all the keys in the base setup array
                {
                    //add key according to the saved string or default
                    inputKeys.Add(baseSetup[i].keyName, (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(baseSetup[i].keyName, baseSetup[i].defaultKey)));

                    //for all the UI Text Change the Display to what the Bind is
                    baseSetup[i].keyDisplayText.text = inputKeys[baseSetup[i].keyName].ToString();
                }
            }            
        }

        // Update is called once per frame
        public void SaveKeys()
        {
            foreach (var key in inputKeys)//
            {
                PlayerPrefs.SetString(key.Key, key.Value.ToString());
            }
            PlayerPrefs.Save();
        }
        public void ChangeKey(GameObject clickedKey)
        {
            currentKey = clickedKey;
            if (clickedKey != null)
            {
                currentKey.GetComponent<Image>().color = selectedKey; //this is just visual to know it's clicked, like a debug
            }
        }
        private void OnGUI()
        {
            string newKey = "";//temp key code name
            Event e = Event.current;//temp current event
            if (currentKey != null)//if we have a key selected to change
            {
                if (e.isKey)//add the event that happens is a key press
                {
                    newKey = e.keyCode.ToString();//hold onto the name of that key
                }
                //if there is an issue getting the Shift Keys the following will fix that
                if (Input.GetKey(KeyCode.LeftShift))//if we press left shift
                {
                    newKey = "LeftShift";//set the name of the key to left shift
                }
                if (Input.GetKey(KeyCode.RightShift))//if we press right shift
                {
                    newKey = "RightShift";//set the name of the key to right shift
                }
                if (newKey != "")//if we have set a key
                {
                    inputKeys[currentKey.name] = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey);
                    //the Above changes out Key in the Dictionary to the Key we Just Pressed
                    currentKey.GetComponentInChildren<Text>().text = newKey;
                    //that changes the Display Text to Match the new key
                    currentKey.GetComponent<Image>().color = changedKey;//to show we change it, ala debug
                    currentKey = null; //reset and wait
                }
            }
        }
    }
}
