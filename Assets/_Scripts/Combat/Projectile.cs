using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private bool _isChasingProjectTile = true;
        [SerializeField]
        private float _flyingSpeed;
        [SerializeField]
        private float _lifeAfterImpact;
        [SerializeField]
        private float _maxLifeTime;
        [SerializeField]
        private GameObject _hitEffect;
        [SerializeField]
        private GameObject[] _destroyOnHitObjects;

        private Health _targetHealth;
        private GameObject _instigator;
        private float _damage;
        private Vector3 _targetPoint;

        // Events

        [SerializeField]
        private UnityEvent OnHit;


        // Methods

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_targetHealth != null && _isChasingProjectTile && !_targetHealth.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * _flyingSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            // Check when to trigger OnHit Effects
            var targetHealth = collision.GetComponent<Health>();

            if (_targetHealth != null && targetHealth != _targetHealth) return;
            if (targetHealth == null || targetHealth.IsDead) return;
            if (collision.gameObject == _instigator) return;

            OhHit();

            targetHealth.TakeDamage(_instigator, _damage);

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject gameObject in _destroyOnHitObjects)
            {
                Destroy(gameObject);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }

        #region SetTarget function overloads
        public void SetTarget(GameObject instigator, float damage, Health target)
        {
            SetTarget(instigator, damage, target, Vector3.zero);
        }
        
        public void SetTarget(GameObject instigator, float damage, Vector3 targetPoint)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        private void SetTarget(GameObject instigator, float damage, Health target, Vector3 targetPoint)
        {
            _instigator = instigator;
            _damage = damage;
            _targetHealth = target;
            _targetPoint = targetPoint;

            Destroy(gameObject, _maxLifeTime);
        }
        #endregion

        private Vector3 GetAimLocation()
        {
            if (_targetHealth == null)
            {
                return _targetPoint;
            }

            //Future problem: cannot get aim location of _targetHealth that doesn't have capsucollider
            CapsuleCollider targetCap = _targetHealth.GetComponent<CapsuleCollider>();

            if (targetCap == null)
            {
                return _targetHealth.transform.position;
            }

            return _targetHealth.transform.position + Vector3.up * targetCap.height / 2;
        }

        private void OhHit()
        {
            OnHit?.Invoke();
            _flyingSpeed = 0f;
        }
    }
}
