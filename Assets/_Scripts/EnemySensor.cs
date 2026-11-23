using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    EnemyScript nav;
    [SerializeField] Transform player;
    public LayerMask targetLayers;
    private void Awake()
    {
        nav = GetComponentInParent<EnemyScript>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>().sneaking)
            {
                return;
            }
            else
            { 
                Vector3 direction = (player.position - transform.position).normalized;
                if(Physics.Raycast(transform.position, direction, out RaycastHit hit, targetLayers))
                {
                    if (hit.collider.CompareTag("Player")) { nav.Investigate(hit); }                    
                }                
            }
        }
    }
}
