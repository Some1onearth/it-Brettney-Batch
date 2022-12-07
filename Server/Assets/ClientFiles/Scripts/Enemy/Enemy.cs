using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    #region Network Variables
    public static int maxEnemies = 10;
    public static Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    private static int nextEnemyId = 1;

    public int id;
    //public EnemyState state;
    //public Player target;
    //public CharacterController controller;
    //public Transform shootOrigin;
    //public float gravity = -9.81f;
    //public float patrolSpeed = 2f;
    //public float chaseSpeed = 8f;
    //public float health;
    //public float maxHealth = 100f;
    //public float detectionRange = 30f;
    //public float shootRange = 15f;
    //public float shootAccuracy = 3;
    //public float idleDuration = 1f;

    //private bool isPatrolRoutineRunning;
    //private float yVelocity = 0;



    #endregion


    public NavMeshAgent enemy;

    public LayerMask whatIsGround;

    //Patrolling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    #region Networking Functions
    private void Start()
    {
        id = nextEnemyId;
        nextEnemyId++;
        enemies.Add(id, this);

      //  state = EnemyState.patrol;
      //  gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
     //   patrolSpeed *= Time.fixedDeltaTime;
      //  chaseSpeed *= Time.fixedDeltaTime;


        }

       

        ////private void LookForPlayer()
        ////{
        ////    foreach 
        ////}

        //public enum EnemyState
        //{
        //    idle,
        //    patrol,
        //    chase,
        //    attack
        //}

        #endregion
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
}
