using UnityEngine;

namespace RPG.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float health;

        public void TakeDamage(float damage) {
            health = Mathf.Max(health - damage, 0f);
            Debug.Log($"Current health: {health}");
        }
    }
}
