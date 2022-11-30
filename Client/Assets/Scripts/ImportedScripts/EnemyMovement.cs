using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public enum AIStates
    {
        Patrol,
        Seek,
    }
    public AIStates state;
    public Transform target;
    public Transform player;
    public Transform WayPointParent;
    protected Transform[] wayPoints;
    public int nextPoint, difficulty;
    public NavMeshAgent agent;
    public float walkSpeed, runSpeed, attackRange, sightRange;
    public float distanceToPoint, changePoint;
    public float stopFromPlayer;
    public void Start()
    {

        //get waypoints array from waypoint parent
        wayPoints = WayPointParent.GetComponentsInChildren<Transform>();
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
        Patrol();
        Seek();
    }
    void Patrol()
    {
        //DO NOT CONTINUE IF NO WAYPOINTS, dead, player in range
        if (wayPoints.Length <= 0 || Vector3.Distance(player.position, transform.position) <= sightRange)
        {
            return;
        }
        state = AIStates.Patrol;
        //Set agent to target
        agent.destination = wayPoints[nextPoint].position;
        agent.speed = walkSpeed;
        distanceToPoint = Vector3.Distance(transform.position, wayPoints[nextPoint].position);
        //are we at the waypoint
        if(distanceToPoint <=changePoint)
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
    }
    void Seek()
    {
        //if the player is out of our sight range and inside our attack range
        if (Vector3.Distance(player.position, transform.position) > sightRange || Vector3.Distance(player.position, transform.position) < attackRange)
        {
            //stop seeking
            return;
        }

        //Set AI state
        state = AIStates.Seek;
        //change speed??? up to you
        agent.speed = runSpeed;
        //Target is player
        agent.destination = player.position;
    }
}
