using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
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

    private void OnValidate()
    {
       
        if (room == null)
        {
            room = GetComponent<Room>();
        }


    }
    public void Start()
    {

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
        Debug.Log("Patrol");
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
        Debug.Log("Sending Movement");
       SendMovement();
    }




    private void SendMovement()
    {
        Debug.Log("Movement Send Method");
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
        Debug.Log("Movement Sent");

    }
}
