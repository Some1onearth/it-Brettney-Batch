using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Room room;
    public NavMeshAgent enemy;

    public LayerMask whatIsGround;

    //Patrolling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Patrolling();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            enemy.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }


    private void SendMovement()
    {
        if (NetworkManager.NetworkManagerInstance.CurrentTick % 2 != 0)
        {
            return;
        }
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.enemyMovement);
        message.AddUShort(room.EnemyId);
        message.AddUShort(NetworkManager.NetworkManagerInstance.CurrentTick);

        message.AddVector3(transform.position);
        message.AddVector3(transform.forward);

        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(message);


    }
}
