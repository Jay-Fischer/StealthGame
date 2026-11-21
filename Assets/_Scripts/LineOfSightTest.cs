using Unity.VisualScripting;
using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{

    [Header("Targeting")]
    [SerializeField] Transform target;
    Navigator nav;
    public LayerMask targetLayers;
    float dot;

    private void Awake()
    {
        nav = GetComponent<Navigator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardDirection = transform.forward;

        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDirection, toTarget);
        if (dot < 0.8f)
        {
            if (Physics.Raycast(transform.position, toTarget, out RaycastHit hit, targetLayers))
                if (hit.collider.CompareTag("Player"))
                {
                    nav.AttemptPursue();
                }
                else
                {
                    nav.pursueDelay = 0f;
                }

        }

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
