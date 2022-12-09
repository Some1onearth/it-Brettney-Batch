using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    private bool[] inputs;

    private void Start()
    {
        //creates inputs array with a length of 7 
        inputs = new bool[7];
    }

    private void Update()
    {

        //if user inputs W key
        if (Input.GetKey(KeyCode.W))
        {
            inputs[0] = true;
        }
        //if user inputs S key
        if (Input.GetKey(KeyCode.S))
        {
            inputs[1] = true;
        }
        //if user inputs A key
        if (Input.GetKey(KeyCode.A))
        {
            inputs[2] = true;
        }
        //if user inputs D key
        if (Input.GetKey(KeyCode.D))
        {
            inputs[3] = true;
        }
        //if user inputs Space key
        if (Input.GetKey(KeyCode.Space))
        {
            inputs[4] = true;
        }
        //if user inputs Left Shift key
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputs[5] = true;
        }
        //if user inputs Left Mouse
        if (Input.GetMouseButtonDown(0)) // attack function
        {
            inputs[6] = true;
        }
    }

    private void FixedUpdate()
    {
        //runs SendInput function
        SendInput();

        //increments through inputs array
        for (int i = 0; i < inputs.Length; i++)
        {
            //sets inputs at index i to false
            inputs[i] = false;
        }
    }

    #region Messages

    private void SendInput()
    {
        Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
        message.AddBools(inputs, false);
        message.AddVector3(camTransform.forward);
        NetworkManager.NetworkManagerInstance.GameClient.Send(message);
    }
    #endregion
}

