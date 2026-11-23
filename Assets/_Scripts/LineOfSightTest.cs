using UnityEngine;

public class LineOfSightTest : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] Transform target;
    [SerializeField] Transform pursuePoint;
    [SerializeField] Transform rayEmmision;
    EnemyScript nav;
    public LayerMask targetLayers;
    float dot;
    float maxDistance = 10;

    private void Awake()
    {
        nav = GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forwardDirection = transform.forward;

        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDirection, toTarget);
        if (dot > 0.7f)
        {
            if (Physics.Raycast(rayEmmision.position, toTarget, out RaycastHit hit, maxDistance, targetLayers))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    pursuePoint.position = new Vector3(hit.collider.transform.position.x, 0, hit.collider.transform.position.z);
                    nav.AttemptPursue();
                }
            } 
        }
        else 
        {
            nav.pursueCheck = false;
        }
    }
}
