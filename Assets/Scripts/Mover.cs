using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    private NavMeshAgent navMeshAgent;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            MoveToCursor();
        }
    }

    private void MoveToCursor() {
        Ray lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(lastRay, out hit);

        if (hasHit) {
            navMeshAgent.destination = hit.point;
        }
    }

}
