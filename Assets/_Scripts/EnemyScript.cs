using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class EnemyScript : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] patrolPoints;
    Transform currentPatrolPoint;
    int PatrolPointIndex;
    [SerializeField] Transform pursuePoint;
    [SerializeField] Transform playerTarget;

    public enum State { patrol, pursue, investigate };
    [Header("States")]
    [SerializeField] public State myState = State.patrol;
    [SerializeField] GameObject Exclamation;
    [SerializeField] GameObject Question;

    [Header("Delays")]
    public float pursueDelay;
    public float investigateDelay;
    public float waitTime;

    [Header("Bools")]
    bool heardSomething;
    public bool pursueCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        currentPatrolPoint = patrolPoints[0];
        agent.SetDestination(currentPatrolPoint.position);     
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
    //Runs every frame while patroling
    void UpdatePatrol()
    {
        if (!pursueCheck && pursueDelay > 0)
        {
            pursueDelay -= Time.deltaTime/2;
        }
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

    //Runs Every frame while pursuing
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            pursueDelay = 0;
            myState = State.investigate;
            Exclamation.SetActive(false);
            Question.SetActive(true);
        }
    }

    //Runs Every Frame in while in the investigate state
    void UpdateInvestigate()
    {
        if(investigateDelay <= 1f)
            investigateDelay += Time.deltaTime;
        else if (investigateDelay > 1f)
        {
            agent.isStopped = false;
            agent.SetDestination(pursuePoint.position);

            float distanceToPoint = Vector3.Distance(transform.position, pursuePoint.position);

            if(distanceToPoint < 1)
            {
                if(waitTime <= 2)
                    waitTime += Time.deltaTime;
                else if(waitTime > 2)
                {
                    Question.SetActive(false);
                    agent.SetDestination(currentPatrolPoint.position);
                    waitTime = 0;
                    investigateDelay = 0;
                    pursueDelay = 0;
                    heardSomething = false;
                    myState = State.patrol;
                }
            }
            else
            {
                if (waitTime != 0) waitTime = 0;
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
            Question.SetActive(true);
        }
    }

    //While player is in Line of Sight
    public void AttemptPursue()
    {
        pursueCheck = true;
        Debug.Log("Attempting Pursue...");
        if(pursueDelay <= 1f)
            pursueDelay += Time.deltaTime;
        else if (pursueDelay > 1f)
        {
            myState = State.pursue;
            agent.isStopped = false;
            Question.SetActive(false);
            Exclamation.SetActive(true);
            heardSomething = false;
        }
    }

    //Stop the Agent's Movement
    public void StopAgent()
    {
        agent.isStopped = true;
    }

}
