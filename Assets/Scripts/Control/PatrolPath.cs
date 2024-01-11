using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour {

        [SerializeField]
        private float waypointGizmosRadius;

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(GetWaypointPosition(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextIndex(i)));
            }
        }

        private int GetNextIndex(int index) {
            if (index == transform.childCount - 1) {
                return 0;
            } else {
                return index + 1;
            }
        }

        private Vector3 GetWaypointPosition(int index) {
            return transform.GetChild(index).position;
        }
    }
}
