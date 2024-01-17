using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {

        [SerializeField]
        private float flyingSpeed;
        private float damage;

        private Health target;

        private void Update() {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * flyingSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.GetComponent<Health>() != target) return;

            collision.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);    
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
        }
    }
}
