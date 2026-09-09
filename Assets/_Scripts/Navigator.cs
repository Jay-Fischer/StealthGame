using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class Navigator : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rb;
    
    [SerializeField] float moveSpeed = 3;
    [SerializeField] Transform target;

    float timesSinceLastDestination;
    NavMeshPath currentPath;
    Queue<Vector3> remainingPoints;
    Vector3 currentCorner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        currentPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (timesSinceLastDestination > 0.5f)
        {
            //agent.SetDestination(target.position);
            //agent.isStopped = true;


            timesSinceLastDestination = 0;

            agent.enabled = true;

            if(agent.CalculatePath(target.position, currentPath))
            {
                Debug.Log("Calculated Path Correctly");

                remainingPoints.Clear();

                foreach (Vector3 point in currentPath.corners)
                {
                    remainingPoints.Enqueue(point);
                }

                currentCorner = remainingPoints.Dequeue();

                agent.enabled = false;
            }
        }

        timesSinceLastDestination += Time.deltaTime;

        Vector3 newForward = (currentCorner - transform.position).normalized;
        newForward.y = 0;
        transform.position = newForward;

        float distanceToNext = Vector3.Distance(transform.position, currentCorner);
        if(distanceToNext > 2)
        {
            if(remainingPoints.Count > 0)
            {
                currentCorner = remainingPoints.Dequeue();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * moveSpeed;
    }

    private void OnDrawGizmos()
    {
        if (currentPath == null)
            return;

        Gizmos.color = Color.yellow;

        foreach(Vector3 point in currentPath.corners)
        {
            Gizmos.DrawSphere(point, 1);
        }
    }
}
