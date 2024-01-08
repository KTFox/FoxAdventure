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
        if (Input.GetMouseButtonDown(1)) {
            MoveToCursor();
        }

        UpdateAnimator();
    }

    private void MoveToCursor() {
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(lastRay, out hit);

        if (hasHit) {
            navMeshAgent.destination = hit.point;
        }
    }

    private void UpdateAnimator() {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        animator.SetFloat(FORWARDSPEED, speed);

        Debug.Log($"Velocity: {velocity} - Local velocity: {localVelocity}");
    }

}
