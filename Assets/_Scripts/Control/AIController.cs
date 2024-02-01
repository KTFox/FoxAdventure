using UnityEngine;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Utility;

namespace RPG.Control {
    public class AIController : MonoBehaviour {

        [SerializeField] 
        private float chaseDistance;
        [SerializeField] 
        private float aggroCooldown;
        [SerializeField] 
        private float shoutDistance;
        [SerializeField] 
        private float suspiciousTime;
        [SerializeField] 
        private float waypointDwellTime;

        [Range(0f, 1f)]
        [SerializeField] 
        private float patrolSpeedFraction;

        [Tooltip("If patrolPath equal null, the enemy will not patrol and stay at guardPosition.")]
        [SerializeField] 
        private PatrolPath patrolPath;

        private ActionScheduler actionScheduler;
        private Fighter fighter;
        private Mover mover;
        private GameObject player;
        private Health health;

        private float waypointTolerance = 1f;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceLastAgrrevated = Mathf.Infinity;
        private bool hasBeenAggroedRecently;
        private int currentWaypointIndex;

        private LazyValue<Vector3> guardPosition;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();

            player = GameObject.FindGameObjectWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetInitGuardPosition);
        }

        Vector3 GetInitGuardPosition() {
            return transform.position;
        }

        private void Start() {
            guardPosition.ForceInit();
        }

        private void Update() {
            if (health.IsDeath) return;

            if (IsAggrevated() && fighter.CanAttack(player)) {
                AttackBehaviour();
            } else if (timeSinceLastSawPlayer < suspiciousTime) {
                SuspiciousBehaviour();
            } else {
                PatrolBehaviour();
            }

            UpdateTimer();
        }

        /// <summary>
        /// return true 
        /// if distanceToPlayer is less than chaseDistance
        /// or if timeSinceLastAgrrevated is less than aggroCooldown
        /// </summary>
        /// <returns></returns>
        private bool IsAggrevated() {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceLastAgrrevated < aggroCooldown;
        }

        private void AttackBehaviour() {
            timeSinceLastSawPlayer = 0f;
            fighter.StartAttackAction(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies() {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);
            foreach (RaycastHit hit in hits) {
                AIController controller = hit.collider.GetComponent<AIController>();
                if (controller == null || controller == this) continue;
                controller.BeAggrevated();
            }
        }

        public void BeAggrevated() {
            if (hasBeenAggroedRecently) return;
            else {
                timeSinceLastSawPlayer = 0f;

                timeSinceLastAgrrevated = 0f;
                hasBeenAggroedRecently = true;
            }
        }

        private void SuspiciousBehaviour() {
            actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = guardPosition.Value;

            if (patrolPath != null) {
                if (AtWaypoint()) {
                    CycleWaypoint();
                    timeSinceArrivedAtWaypoint = 0f;
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime) {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
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
            timeSinceLastAgrrevated += Time.deltaTime;

            if (timeSinceLastAgrrevated >= aggroCooldown && timeSinceLastSawPlayer >= suspiciousTime) {
                hasBeenAggroedRecently = false;
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
            Gizmos.DrawWireSphere(transform.position, shoutDistance);
        }
    }
}
