using UnityEngine;

namespace RPG
{
    public class DisplayCast : MonoBehaviour
    {
        [SerializeField]
        private float areaAffectedRadius;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;    
            Gizmos.DrawWireSphere(transform.position, areaAffectedRadius);
        }
    }
}
