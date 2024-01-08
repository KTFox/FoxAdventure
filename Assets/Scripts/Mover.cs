using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        navMeshAgent.destination = target.position;
    }

}
