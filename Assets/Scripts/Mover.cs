using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    private const string FORWARDSPEED = "forwardSpeed";

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        UpdateAnimator();
    }

    public void MoveToCursor() {
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(lastRay, out hit);

        if (hasHit) {
            MoveTo(hit.point);
        }
    }

    public void MoveTo(Vector3 position) {
        navMeshAgent.destination = position;
    }

    private void UpdateAnimator() {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        animator.SetFloat(FORWARDSPEED, speed);
    }

}
