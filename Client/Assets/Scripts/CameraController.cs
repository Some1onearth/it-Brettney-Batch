using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float clampAngle = 85f;

    private float verticalRotation;
    private float horizontalRotation;


    private void OnValidate()
    {
        if (player == null)
        {
            player = GetComponentInParent<Player>();
        }
    }

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = player.transform.eulerAngles.y;

    }



    private void Update()
    {
        //if esc is pressed this code will operate
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }

       //if the cursor is not moving and is in a "lockstate" the Look code will operate
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);



    }

    
    private void Look()
    {
        //The in-game camera will follow the player's mouse movement, allows the player to look around

        float mouseVertical = -Input.GetAxis("Mouse Y"); 
        float mouseHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += mouseVertical * sensitivity * Time.deltaTime;
        horizontalRotation += mouseHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }


    private void ToggleCursorMode()
    {
        //The camera will no longer register to the player's mouse movement
        //and instead appear a cursor 
        Cursor.visible = !Cursor.visible;


        //checks if the mouse is locked at the center of the screen
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        else 
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

}
