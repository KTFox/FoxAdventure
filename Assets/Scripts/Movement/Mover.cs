using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {

        #region Animation strings
        private const string FORWARDSPEED = "forwardSpeed";
        #endregion

        #region Caching variables
        private ActionScheduler actionScheduler;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;
        #endregion

        [SerializeField]
        private float maxSpeed;

        private void Start() {
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        private void Update() {
            navMeshAgent.enabled = !health.IsDeath();

            UpdateAnimator();
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat(FORWARDSPEED, speed);
        }

        /// <summary>
        /// Call ActionScheduler.StartAction() and MoveTo(destination) functions
        /// </summary>
        /// <param name="destination"></param>
        public void StartMoveAction(Vector3 destination, float moveSpeedFraction) {
            actionScheduler.StartAction(this);
            MoveTo(destination, moveSpeedFraction);
        }

        /// <summary>
        /// Set navMeshAgent.destination equal position
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Vector3 position, float moveSpeedFraction) {
            navMeshAgent.destination = position;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(moveSpeedFraction);
            navMeshAgent.isStopped = false;
        }

        #region IAction Interface implements
        /// <summary>
        /// Set navMeshAgent.isStopped = true
        /// </summary>
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }
        #endregion

    }
}

