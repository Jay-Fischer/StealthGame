using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{
    [SerializeField] Transform target;

    enum State {idle, patrol, pursue, dead};

    State myState = State.idle;

    float dot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case State.idle:
                UpdateIdle(); break;
            case State.patrol:
                UpdatePatrol(); break;
            case State.pursue:
                UpdatePursue(); break;
            case State.dead:
                UpdateDead(); break;
        }


        
    }

    void UpdateIdle()
    {
        Vector3 forwardDirection = transform.forward;

        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDirection, toTarget);
        if (dot < 0.8f)
        {
            myState = State.pursue;
        }
    }

    void UpdatePatrol()
    {
        transform.Translate(transform.forward * -10 * Time.deltaTime);
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance > 10)
        {
            UpdateDead();
        }
    }

    void UpdatePursue()
    {

    }

    void UpdateDead()
    {
        Debug.Log("Oh no, I am dead.");
    }





    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position);

        Vector3 endPoint = transform.position + transform.forward * 10;

        Gizmos.color = Color.red;
        if (dot > 0.8f)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
