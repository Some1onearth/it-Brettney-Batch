using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Character _char;
    [SerializeField] private GameObject _rangedAttackPrefab;
    private Transform _attackPosition;

    private bool[] inputs;

    private void Start()
    {
        inputs = new bool[6];
    }




    private void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            inputs[0] = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputs[1] = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputs[2] = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputs[3] = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            inputs[4] = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputs[5] = true;
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    inputs[6] = true;
        //}

    }

    private void Attack(Transform target)
    {
        if (target != null)
        {
            Debug.Log("Attacking Target!");
            GameObject rangedProjectile = Instantiate(_rangedAttackPrefab, _attackPosition.position, Quaternion.identity);
            // var rangedProjectileBehaviour = rangedProjectile.GetComponent<ProjectileBehaviour>();
            // rangedProjectileBehaviour.Target = target;
            // rangedProjectileBehaviour.Speed = _char.characterData.attackSpeed;
            // rangedProjectileBehaviour.Damage = _char.characterData.attackDamage;
        }
        else
        {
            Debug.Log("No Target!");
        }
    }


    private void FixedUpdate()
    {
        SendInput();


        for (int i = 0; i < inputs.Length; i++)
        {
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

