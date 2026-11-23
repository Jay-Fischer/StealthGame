using Unity.VisualScripting;
using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{

    [Header("Targeting")]
    [SerializeField] Transform target;
    [SerializeField] Transform pursuePoint;
    [SerializeField] Transform rayEmmision;
    Navigator nav;
    public LayerMask targetLayers;
    float dot;
    float maxDistance = 10;

    private void Awake()
    {
        nav = GetComponent<Navigator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardDirection = transform.forward;

        Vector3 toTarget = (target.position - transform.position).normalized;

        Debug.DrawRay(rayEmmision.position, toTarget * maxDistance, Color.red);
        dot = Vector3.Dot(forwardDirection, toTarget);
        if (dot > 0.8f)
        {
            Debug.Log("Within LoS");
            
            if (Physics.Raycast(rayEmmision.position, toTarget, out RaycastHit hit, maxDistance, targetLayers))
            {
                pursuePoint.position = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                nav.AttemptPursue();
                Debug.Log("Hit Player");
            }
                
        }
    }

    /*
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
    }*/
}
