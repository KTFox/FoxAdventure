using UnityEngine;
using RPG.Movement;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] private float weaponRange = 5f;

        private Transform target;
        private Mover mover;

        private void Awake() {
            mover = GetComponent<Mover>();
        }

        private void Update() {
            bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;

            if (target != null && !isInRange) {
                mover.MoveTo(target.position);
            } else {
                mover.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget) {
            target = combatTarget.transform;
        }
    }
}