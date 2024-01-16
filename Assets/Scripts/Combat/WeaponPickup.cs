using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {

        [SerializeField]
        private WeaponSO weaponSO;

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<Fighter>().EquipWeapon(weaponSO);

                Destroy(gameObject);
            }
        }
    }
}
