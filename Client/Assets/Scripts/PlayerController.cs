using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    [SerializeField] private GameObject _projectileSpawner;
    [SerializeField] private float timer = 0, shootTimer;

    [SerializeField] private Transform camTransform;
    [SerializeField] private List<MobileButton> _mobileButtons;
    [SerializeField] private MobileButton _upButton, _downButton, _leftButton, _rightButton, _attackButton;

    public bool upButton, downButton, leftButton, rightButton, attackButton;

    private bool[] inputs;


    private void Awake()
    {
        _upButton = GameObject.FindWithTag("UpButton").GetComponent<MobileButton>();
        _mobileButtons.Add(_upButton);
        _downButton = GameObject.FindWithTag("DownButton").GetComponent<MobileButton>();
        _mobileButtons.Add(_downButton);
        _leftButton = GameObject.FindWithTag("LeftButton").GetComponent<MobileButton>();
        _mobileButtons.Add(_leftButton);
        _rightButton = GameObject.FindWithTag("RightButton").GetComponent<MobileButton>();
        _mobileButtons.Add(_rightButton);
        _attackButton = GameObject.FindWithTag("AttackButton").GetComponent<MobileButton>();
        _mobileButtons.Add(_attackButton);
    }

    private void Start()
    {
        foreach (var button in _mobileButtons)
        {
            button._pc = this;
        }
        //creates inputs array with a length of 7 
        inputs = new bool[7];
    }

    public void ShootBullet()
    {
        if (Input.GetMouseButton(1) || attackButton)
        {
            if(Time.realtimeSinceStartup - timer >= 1)
            {
                timer = Time.realtimeSinceStartup;
                Instantiate(projectilePrefab, _projectileSpawner.transform.position, Quaternion.identity);
            }
        }
    }

    private void Update()
    {

        ShootBullet();

        //if user inputs W key
        if (Input.GetKey(KeyCode.W) || upButton)
        {
            inputs[0] = true;
        }
        //if user inputs S key
        if (Input.GetKey(KeyCode.S) || downButton)
        {
            inputs[1] = true;
        }
        //if user inputs A key
        if (Input.GetKey(KeyCode.A) || leftButton)
        {
            inputs[2] = true;
        }
        //if user inputs D key
        if (Input.GetKey(KeyCode.D) || rightButton)
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
        if (Input.GetMouseButtonDown(1) || attackButton) // attack function
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

