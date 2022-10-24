using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform camProxy;
    [SerializeField] private float gravity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;

    private float gravityAcceleration;
    private float moveSpeed;
    private float jumpSpeed;

    private bool[] inputs;
    private float yVelocity;

    





    private void OnValidate()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
        if (player == null)
        {
            player = GetComponent<Player>();
        }



        gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed = movementSpeed * Time.fixedDeltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        inputs = new bool[6];
    }

    private void FixedUpdate()
    {

        Vector2 inputDirection = Vector2.zero;
        if (inputs[0])
        {
            inputDirection.y += 1;
        }

        if (inputs[1])
        {
            inputDirection.y -= 1;
        }

        if (inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            inputDirection.x += 1;
        }

        Move(inputDirection, inputs[4], inputs[5]);


    }

    private void Initialize()
    {
        gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed = movementSpeed * Time.fixedDeltaTime;
        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);

    }
    // Update is called once per frame
    private void Move(Vector2 inputDirection, bool jump, bool sprint)
    {

//       Set the player's forward rotation to the inputDirection (what way the character is facing)
//       Make the player Move forward regardless while input is being used.
//       
      
if (inputDirection.magnitude>0)
        {
            Vector3 lookDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
            transform.forward = lookDirection;
            //
            Vector3 moveDirection = transform.forward;
            moveDirection *= moveSpeed;
            if (sprint)
            {
                moveDirection *= 2f;

            }

            if (controller.isGrounded)
            {
                yVelocity = 0f;
                if (jump)
                {
                    yVelocity = jumpSpeed;
                }
            }
            yVelocity += gravityAcceleration;

            moveDirection.y = yVelocity;
            controller.Move(moveDirection);

        }


        SendMovement();
    }



    private Vector3 FlattenVector3(Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }

    public void SetInput(bool[] inputs, Vector3 forward)
    {
        this.inputs = inputs;
        camProxy.forward = forward;
    }





    private void SendMovement()
    {
        if (NetworkManager.NetworkManagerInstance.CurrentTick % 2 != 0)
        {
            return;
        }
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.playerMovement);
        message.AddUShort(player.Id);
        message.AddUShort(NetworkManager.NetworkManagerInstance.CurrentTick);

        message.AddVector3(transform.position);
        message.AddVector3(transform.forward);
      
        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(message);


    }
}
