using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        // Variables

        private const string FORWARD_SPEED = "forwardSpeed";

        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        private float _maxNavPathLength;

        private ActionScheduler _actionScheduler;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Health _health;


        // Methods

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float moveSpeedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, moveSpeedFraction);
        }

        public void MoveTo(Vector3 destination, float moveSpeedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = _maxSpeed * Mathf.Clamp01(moveSpeedFraction);
            _navMeshAgent.isStopped = false;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath)
            {
                return false;
            }

            if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            if (GetNavPathLength(path) > _maxNavPathLength)
            {
                return false;
            }

            return true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            _animator.SetFloat(FORWARD_SPEED, speed);
        }

        private float GetNavPathLength(NavMeshPath path)
        {
            float pathLength = 0f;

            if (path.corners.Length < 2)
            {
                return pathLength;
            }

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                pathLength += distance;
            }

            return pathLength;
        }

        #region IAction Interface implements
        /// <summary>
        /// Set _navMeshAgent.isStopped = true
        /// </summary>
        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
        #endregion

        #region ISaveable Interface implements
        object ISaveable.CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        void ISaveable.RestoreState(object state)
        {
            _navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3)(state)).ToVector();
            _navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        #endregion
    }
}

