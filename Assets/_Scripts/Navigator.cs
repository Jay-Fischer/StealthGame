using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class Navigator : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    Transform currentPatrolPoint;
    int PatrolPointIndex;


    enum State { patrol, pursue, investigate };
    [Header("States")]
    [SerializeField] State myState = State.patrol;

    [Header("Delays")]
    public float pursueDelay;
    public float investigateDelay;




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

    }

    void UpdateInvestigate()
    {
        investigateDelay += Time.deltaTime;
        if (investigateDelay > 0.5f)
        {
            
        }
    }

    public void Investigate(RaycastHit hit)
    {
        if (myState != State.pursue)
        {
            Debug.Log("What Was That?");
            StopAgent();
        }
    }

    public void AttemptPursue()
    {
        pursueDelay += Time.deltaTime;
        if (pursueDelay > 1f)
        {
            myState = State.pursue;
        }
    }
    public void StopAgent()
    {
        agent.isStopped = true;
    }

}
