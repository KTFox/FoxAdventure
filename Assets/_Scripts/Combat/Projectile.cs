using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {

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

        private void Start() {
            transform.LookAt(GetAimLocation());
        }

        private void Update() {
            if (target == null) return;

            if (isChasingProjectTile && !target.IsDeath) {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * flyingSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.GetComponent<Health>() != target) return;
            if (target.IsDeath) return;

            OnHit?.Invoke();

            target.TakeDamage(instigator, damage);
            flyingSpeed = 0f;

            if (hitEffect != null) {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject gameObject in destroyOnHitObjects) {
                Destroy(gameObject);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        private Vector3 GetAimLocation() {
            //Future problem: cannot get aim location of target that doesn't have capsucollider
            CapsuleCollider targetCap = target.GetComponent<CapsuleCollider>();

            if (targetCap == null) {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCap.height / 2;
        }
        
        /// <summary>
        /// Set target, instigator and damage
        /// </summary>
        /// <param name="target"></param>
        /// <param name="instigator"></param>
        /// <param name="damage"></param>
        public void SetTarget(Health target, GameObject instigator, float damage) {
            this.target = target;
            this.instigator = instigator;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }
    }
}
