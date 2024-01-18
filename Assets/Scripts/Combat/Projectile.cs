using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {

        [SerializeField]
        private float flyingSpeed;
        [SerializeField]
        private bool isChasingProjectTile = true;
        [SerializeField]
        private GameObject hitEffect;
        [SerializeField]
        private GameObject[] destroyOnHit;
        [SerializeField]
        private float lifeAfterImpact;
        [SerializeField]
        private float maxLifeTime;

        private float damage;

        private Health target;

        private void Start() {
            transform.LookAt(GetAimLocation());
        }

        private void Update() {
            if (target == null) return;

            if (isChasingProjectTile && !target.IsDeath()) {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * flyingSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.GetComponent<Health>() != target) return;
            if (target.IsDeath()) return;

            target.TakeDamage(damage);
            flyingSpeed = 0f;

            if (hitEffect != null) {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject gameObject in destroyOnHit) {
                Destroy(gameObject);
            }

            Destroy(gameObject, lifeAfterImpact);
        }

        private Vector3 GetAimLocation() {
            CapsuleCollider targetCap = target.GetComponent<CapsuleCollider>();

            if (targetCap == null) {
                return target.transform.position;
            }

            return target.transform.position + Vector3.up * targetCap.height / 2;
        }

        public void SetTarget(Health target, float damage) {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }
    }
}
