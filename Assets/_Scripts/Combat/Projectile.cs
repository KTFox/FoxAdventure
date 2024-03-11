using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        private bool isChasingProjectTile = true;
        [SerializeField]
        private float flyingSpeed;
        [SerializeField]
        private float lifeAfterImpact;
        [SerializeField]
        private float maxLifeTime;
        [SerializeField]
        private GameObject hitEffect;
        [SerializeField]
        private GameObject[] destroyOnHitObjects;
        [SerializeField]
        private UnityEvent OnHit;

        private Health target;
        private GameObject instigator;
        private float damage;
        private Vector3 targetPoint;
        #endregion

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (target != null && isChasingProjectTile && !target.IsDeath)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * flyingSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            // Check when to trigger Hit Effects
            Health targetHealth = collision.GetComponent<Health>();
            if (target != null && targetHealth != target) return;
            if (targetHealth == null || targetHealth.IsDeath) return;
            if (collision.gameObject == instigator) return;

            // Trigger Hit Effects
            OnHit?.Invoke();
            targetHealth.TakeDamage(instigator, damage);
            flyingSpeed = 0f;
            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }
            foreach (GameObject gameObject in destroyOnHitObjects)
            {
                Destroy(gameObject);
            }
            Destroy(gameObject, lifeAfterImpact);
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
            this.instigator = instigator;
            this.damage = damage;
            this.target = target;
            this.targetPoint = targetPoint;

            Destroy(gameObject, maxLifeTime);
        }
        #endregion

        private Vector3 GetAimLocation()
        {
            if (target == null)
            {
                return targetPoint;
            }

            //Future problem: cannot get aim location of target that doesn't have capsucollider
            CapsuleCollider targetCap = target.GetComponent<CapsuleCollider>();

            if (targetCap == null)
            {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCap.height / 2;
        }
    }
}
