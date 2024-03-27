using UnityEngine;
using System;
using UnityEngine.AI;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Utility;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private float _chaseDistance;
        [SerializeField]
        private float _aggroCooldown;
        [SerializeField]
        private float _shoutDistance;
        [SerializeField]
        private float _suspiciousTime;
        [SerializeField]
        private float _waypointDwellTime;

        [Range(0f, 1f)]
        [SerializeField]
        private float _patrolSpeedFraction;

        [Tooltip("If _patrolPath equal null, the enemy will not patrol and stay at guardPosition.")]
        [SerializeField]
        private PatrolPath _patrolPath;

        private ActionScheduler _actionScheduler;
        private Fighter _fighter;
        private Mover _mover;
        private Health _health;
        private GameObject _player;

        private LazyValue<Vector3> guardPosition;

        private float _waypointTolerance = 1f;
        private float _timeSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float _timeSinceLastAgrrevated = Mathf.Infinity;
        private int _currentWaypointIndex;
        private bool _hasBeenAggroedRecently;


        // Methods

        private void Awake()
        {
            _actionScheduler = GetComponent<ActionScheduler>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _health = GetComponent<Health>();

            _player = GameObject.FindGameObjectWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetDefaultGuardPosition);
        }

        Vector3 GetDefaultGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;

            UpdateTimers();

            if (IsAggrevated() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
        }

        public void BeAggrevated()
        {
            if (_hasBeenAggroedRecently)
            {
                return;
            }
            else
            {
                _timeSinceLastSawPlayer = 0f;
                _timeSinceLastAgrrevated = 0f;
                _hasBeenAggroedRecently = true;
            }
        }

        public void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition.Value);

            _timeSinceLastSawPlayer = Mathf.Infinity;
            _timeSinceArrivedAtWaypoint = Mathf.Infinity;
            _timeSinceLastAgrrevated = Mathf.Infinity;
            _currentWaypointIndex = 0;
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

            return distanceToPlayer < _chaseDistance || _timeSinceLastAgrrevated < _aggroCooldown;
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0f;
            _fighter.StartAttackAction(_player);
            AggrevateNearbyEnemies();
        }

        private void SuspiciousBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextWaypointPosition = guardPosition.Value;

            if (_patrolPath != null)
            {
                if (HasReachedWaypoint())
                {
                    CycleWaypoint();
                    _timeSinceArrivedAtWaypoint = 0f;
                }

                nextWaypointPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextWaypointPosition, _patrolSpeedFraction);
            }
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                var nearbyEnemyController = hit.collider.GetComponent<AIController>();

                if (nearbyEnemyController == null || nearbyEnemyController == this) continue;

                nearbyEnemyController.BeAggrevated();
            }
        }

        private bool HasReachedWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNextWaypointIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypointPosition(_currentWaypointIndex);
        }

        private void UpdateTimers()
        {
            Debug.Log(_currentWaypointIndex);

            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceLastAgrrevated += Time.deltaTime;

            if (_timeSinceLastAgrrevated >= _aggroCooldown && _timeSinceLastSawPlayer >= _suspiciousTime)
            {
                _hasBeenAggroedRecently = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
            Gizmos.DrawWireSphere(transform.position, _shoutDistance);
        }
    }
}
