using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction {

        private const string FORWARDSPEED = "forwardSpeed";

        private ActionScheduler actionScheduler;
        private NavMeshAgent navMeshAgent;
        private Animator animator;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void Update() {
            UpdateAnimator();
        }

        /// <summary>
        /// Call ActionScheduler.StartAction() and MoveTo(destination) functions
        /// </summary>
        /// <param name="destination"></param>
        public void StartMoveAction(Vector3 destination) {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        /// <summary>
        /// Set navMeshAgent.destination equal position
        /// </summary>
        /// <param name="position"></param>
        public void MoveTo(Vector3 position) {
            navMeshAgent.destination = position;
            navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// Set navMeshAgent.isStopped = true
        /// </summary>
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat(FORWARDSPEED, speed);
        }
    }
}

