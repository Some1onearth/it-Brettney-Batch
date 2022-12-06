using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
//Access Modifiers   //camelCasing
    [Header ("Player Health")]
    public float curHealth;
    public float maxHealth = 100;
    [Header("Lighting")]
    public Light sun;
    [Range (-2.5f,2.5f)]
    public float brightness = 2.0f;
    
    [Space(20)]//gives space between public components

    #region  Variables
    public string groupingOfCharacters = "Laeyd^@&#$*@#@928e9hdiufbla;'af ";
    public bool trueOrFalse;//boolean
    public int wholeNumber;
    public float decimalNumber;//floating point
    #endregion

    //single comment
    /*
     * Paragraph Comment
     */

    public GameObject gO;
    public Transform t;

    //Struct
    public Vector2 xAndY;//2 floats
    public Vector3 xyAndZ;//3 floats
    public Quaternion xyzAndW;//4 floats

    [System.Serializable]
    public struct ExampleStruct
    {
        [Header("CHANGE NAME HERE")]
        public string name;
        public GameObject myBody;
        public Vector3 myPos;
        public int age;
        public float height;
        public bool isdead;
    };
    //array, lists and dictionaries
    public ExampleStruct[] exampleStruct;


    // Start is called before the first frame update
    void Start()
    {
        brightness = sun.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
