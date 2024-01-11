using RPG.Combat;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField]
        private float chaseDistance;

        private Fighter fighter;
        private GameObject player;

        private void Start() {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update() {
            if (PlayerInAttackRange() && fighter.CanAttack(player)) {
                fighter.Attack(player);
            } else {
                fighter.Cancel();
            }
        }

        private bool PlayerInAttackRange() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}
