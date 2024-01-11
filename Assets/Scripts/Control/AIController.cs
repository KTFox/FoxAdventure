using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseDistance;

        private void Update() {
            if (GetDistanceToPlayer() <= chaseDistance) {
                Debug.Log($"{this.name} is Chasing player");
            }
        }

        private float GetDistanceToPlayer() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            return distanceToPlayer;
        }
    }
}
