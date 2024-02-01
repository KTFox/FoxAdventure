using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement {
    public class Mover : MonoBehaviour, IAction, ISaveable {

        [SerializeField] 
        private float maxSpeed;
        [SerializeField] 
        private float maxNavPathLength;

        private ActionScheduler actionScheduler;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;

        private void Awake() {
            actionScheduler = GetComponent<ActionScheduler>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        private void Update() {
            navMeshAgent.enabled = !health.IsDeath;

            UpdateAnimator();
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat("forwardSpeed", speed);
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

        public bool CanMoveTo(Vector3 position) {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, position, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetNavPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetNavPathLength(NavMeshPath path) {
            float pathLength = 0f;

            if (path.corners.Length < 2) return pathLength;

            for (int i = 0; i < path.corners.Length - 1; i++) {
                float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                pathLength += distance;
            }

            return pathLength;
        }

        #region IAction Interface implements
        /// <summary>
        /// Set navMeshAgent.isStopped = true
        /// </summary>
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }
        #endregion

        #region ISaveable Interface implements
        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3)(state)).ToVector();
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        #endregion
    }
}
