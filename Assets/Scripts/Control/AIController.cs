using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField]
        private float chaseDistance;

        [SerializeField]
        private float suspiciousTime;
        private float timeSinceLastSawPlayer = Mathf.Infinity;

        [SerializeField]
        private PatrolPath patrolPath;
        [SerializeField]
        private float waypointDwellTime;
        private int currentWaypointIndex;
        private float waypointTolerance = 1f;
        private Vector3 guardPosition;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        #region Caching variables
        private ActionScheduler actionScheduler;
        private Fighter fighter;
        private Mover mover;
        private GameObject player;
        private Health health;
        #endregion

        private void Start() {
            actionScheduler = GetComponent<ActionScheduler>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player");
            health = GetComponent<Health>();

            guardPosition = transform.position;
        }

        private void Update() {
            if (health.IsDeath()) return;

            if (PlayerInAttackRange() && fighter.CanAttack(player)) {
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspiciousTime) {
                SuspiciousBehaviour();
            } else {
                PatrolBehaviour();
            }

            UpdateTimer();
        }

        private bool PlayerInAttackRange() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0f;
            fighter.StartAttackAction(player);
        }

        private void SuspiciousBehaviour() {
            actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null) {
                if (AtWaypoint()) {
                    CycleWaypoint();
                    timeSinceArrivedAtWaypoint = 0f;
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint() {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint() {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypointPosition(currentWaypointIndex);
        }

        private void UpdateTimer() {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
