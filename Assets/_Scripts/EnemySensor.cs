using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemySensor : MonoBehaviour
{
    Navigator nav;
    [SerializeField] Transform player;
    public LayerMask targetLayers;
    private void Awake()
    {
        nav = GetComponentInParent<Navigator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().sneaking)
            {
                Debug.Log("Nothing To See Here");
                return;
            }
            else
            { 
                Vector3 direction = (player.position - transform.position).normalized;
                if(Physics.Raycast(transform.position, direction, out RaycastHit hit, targetLayers))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.blue);
                    if (hit.collider.CompareTag("Player")) { nav.Investigate(hit); }                    
                }                
            }
        }
    }
}
