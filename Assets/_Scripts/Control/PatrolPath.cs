using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        // Variables

        private float _waypointGizmosRadius = 1f;


        // Methods

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), _waypointGizmosRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextWaypointIndex(i)));
            }
        }

        public int GetNextWaypointIndex(int currentWaypointIndex)
        {
            if (currentWaypointIndex == transform.childCount - 1)
            {
                return 0;
            }
            else
            {
                return currentWaypointIndex + 1;
            }
        }

        public Vector3 GetWaypointPosition(int waypointIndex)
        {
            return transform.GetChild(waypointIndex).position;
        }
    }
}
