using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //This script Handles ONLY enemyMovement and Death
    public ushort EnemyId { get; set; } //Used to get and set the EnemyId in room.list
    public static Room room;
    public enum AIStates
    {
        Patrol,
        Seek,
    }
    public AIStates state;
    public Transform target;
    public GameObject wayPointPrefab;
    public Transform WayPointParent;
    public Transform wayPointSpawnParent;
    protected Transform[] wayPoints;
    public int nextPoint, difficulty;
    public NavMeshAgent agent;
    public float walkSpeed, runSpeed, attackRange, sightRange;
    public float distanceToPoint, changePoint;
    // public float stopFromPlayer;
    private ushort internalID;

    private void OnValidate()
    {
        if (room == null)
        {
            room = GetComponent<Room>();
        }
    }
    public void Start()
    {
        internalID = EnemyId; //Sets the internal ID to the unique enemy.
        wayPointSpawnParent = GameObject.Find("EnemyWaypoints").transform;
        //Instantiate waypoints for the enemy on start
        GameObject spawnedWaypoints = Instantiate(wayPointPrefab, transform.position, Quaternion.identity, wayPointSpawnParent);
        //get waypoints array from waypoint parent
        wayPoints = spawnedWaypoints.GetComponentsInChildren<Transform>();//WayPointParent.GetComponentsInChildren<Transform>();
        //get navMeshAgent from self
        agent = GetComponent<NavMeshAgent>();
        //Set speed of agent
        agent.speed = walkSpeed;
        //Set target Waypoint
        nextPoint = 1;
        //Set Patrol as Default
        Patrol();
    }
    public void Update()
    {
        // SendMovement();
        //  Debug.Log("Patrol");
        Patrol();
    }
    void Patrol()
    {
        //DO NOT CONTINUE IF NO WAYPOINTS, dead, player in range
        //if (wayPoints.Length <= 0 || Vector3.Distance(player.position, transform.position) <= sightRange)
        //{
        //    return;
        //}
        state = AIStates.Patrol;
        //Set agent to target
        agent.destination = wayPoints[nextPoint].position;
        agent.speed = walkSpeed;
        distanceToPoint = Vector3.Distance(transform.position, wayPoints[nextPoint].position);
        //are we at the waypoint
        if (distanceToPoint <= changePoint)
        {
            //if so go to next waypoint
            if (nextPoint < wayPoints.Length - 1)
            {
                nextPoint++;
            }
            //if at end of patrol go to start
            else
            {
                nextPoint = 1;
            }
        }
        // Debug.Log("Sending Movement");
        SendMovement();
    }




    private void SendMovement()
    {
        //SENDS THE MESSAGE CONVERTING internalID, transform.position & transform.forward to a string
        Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.enemyMovement);
        message.AddString(internalID + "|" + transform.position + "|" + transform.forward);
        message.AddUShort(NetworkManager.NetworkManagerInstance.CurrentTick);
        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(message);
        //  Debug.Log(internalID + "|" + transform.position + "|" + transform.forward);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//We will send a message to the client containing what enemy was hit, and the new player Score.
        {
            Debug.Log("Collission with Player");
            EnemyDead();
           Destroy(this.gameObject);
            
        }
    }


    private void EnemyDead()
    {
//Sends a Message through enemyDeath with the internalID of the enemy that is being killed, Sent reliable to ensure message goes through.
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.enemyDeath);
        message.AddUShort(internalID);
        message.AddUShort(NetworkManager.NetworkManagerInstance.CurrentTick);

        NetworkManager.NetworkManagerInstance.GameServer.SendToAll(message);//Sends it to all clients
        Debug.Log("EnemyDead message sent");

    }


}
