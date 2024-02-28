using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private float waypointGizmosRadius = 0.3f;

        public int WaypointCount
        {
            get
            {
                return transform.childCount;
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }

        /// <summary>
        /// Return the next point of current waypoint.
        /// If current waypoint is the end of patrol path, return the first waypoint
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetNextIndex(int index)
        {
            if (index == transform.childCount - 1)
            {
                return 0;
            }
            else
            {
                return index + 1;
            }
        }

        /// <summary>
        /// Return current waypoint position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetWaypointPosition(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}
