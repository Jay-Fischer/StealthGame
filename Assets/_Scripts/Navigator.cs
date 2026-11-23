using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;


public class Navigator : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    Transform currentPatrolPoint;
    int PatrolPointIndex;
    [SerializeField] Transform pursuePoint;
    [SerializeField] Transform playerTarget;

    enum State { patrol, pursue, investigate };
    [Header("States")]
    [SerializeField] State myState = State.patrol;

    [Header("Delays")]
    public float pursueDelay;
    public float investigateDelay;
    public float waitTime;

    [Header("Bools")]
    bool heardSomething;

    NavMeshPath currentPath;
    Queue<Vector3> remainingPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentPatrolPoint = patrolPoints[0];
        agent.SetDestination(currentPatrolPoint.position);

        currentPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();        
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case State.patrol:
                UpdatePatrol(); break;
            case State.pursue:
                UpdatePursue(); break;
            case State.investigate:
                UpdateInvestigate(); break;
        }

    }
    void UpdatePatrol()
    {
        float distance = Vector3.Distance(transform.position, currentPatrolPoint.position);

        if (distance < 1)
        {
            PatrolPointIndex++;
            if (PatrolPointIndex >= patrolPoints.Length)
            {
                PatrolPointIndex = 0;
            }
            currentPatrolPoint = patrolPoints[PatrolPointIndex];
            agent.SetDestination(currentPatrolPoint.position);

        }
    }

    void UpdatePursue()
    {
        float distanceToPoint = Vector3.Distance(transform.position, pursuePoint.position);
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
        if (distanceToPoint > 0.5f)
        {
            agent.SetDestination(pursuePoint.position);
        }
        else if (distanceToPlayer < 0.5f)
        {
            
        }
        else
        {
            pursueDelay = 0;
            myState = State.investigate;
        }
    }

    void UpdateInvestigate()
    {
        investigateDelay += Time.deltaTime;
        if (investigateDelay > 1)
        {
            agent.isStopped = false;
            agent.SetDestination(pursuePoint.position);

            float distanceToPoint = Vector3.Distance(transform.position, pursuePoint.position);

            if(distanceToPoint < 1)
            {
                waitTime += Time.deltaTime;
                if(waitTime > 2)
                {
                    agent.SetDestination(currentPatrolPoint.position);
                    waitTime = 0;
                    investigateDelay = 0;
                    pursueDelay = 0;
                    myState = State.patrol;
                }
            }
        }
    }

    //Start Investigating a point where the player was heard
    public void Investigate(RaycastHit hit)
    {
        if (myState != State.pursue)
        {
            if(heardSomething == false)
            {
                StopAgent();
                heardSomething = true;
            }
            Debug.Log("What Was That?");
            pursuePoint.position = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
            myState = State.investigate;
        }
    }

    //While player is in Line of Sight
    public void AttemptPursue()
    {
        Debug.Log("Attempting Pursue...");
        pursueDelay += Time.deltaTime;
        if (pursueDelay > 2f)
        {
            myState = State.pursue;
            heardSomething = false;
        }
    }
    public void StopAgent()
    {
        agent.isStopped = true;
    }

}
