using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;

namespace RPG.Movement {
    public class Mover : MonoBehaviour {
        private const string FORWARDSPEED = "forwardSpeed";

        private NavMeshAgent navMeshAgent;
        private Fighter fighter;
        private Animator animator;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();  
            animator = GetComponent<Animator>();
        }

        private void Update() {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination) {
            fighter.Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 position) {
            navMeshAgent.destination = position;
            navMeshAgent.isStopped = false;
        }

        public void Stop() {
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

