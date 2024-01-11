using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField]
        private float chaseDistance;

        private Fighter fighter;
        private Mover mover;
        private GameObject player;
        private Health health;

        private Vector3 guardPosition;

        private void Start() {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();

            guardPosition = transform.position;
        }

        private void Update() {
            if (health.IsDeath()) return;

            if (PlayerInAttackRange() && fighter.CanAttack(player)) {
                fighter.StartAttackAction(player);
            } else {
                mover.StartMoveAction(guardPosition);
            }
        }

        private bool PlayerInAttackRange() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
