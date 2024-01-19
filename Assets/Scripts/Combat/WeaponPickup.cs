using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {

        [SerializeField]
        private WeaponSO weaponSO;
        [SerializeField]
        private float hideTime;

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player")) {
                Pickup(collision.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter) {
            fighter.EquipWeapon(weaponSO);
            StartCoroutine(HideForSeconds(hideTime));
        }

        private IEnumerator HideForSeconds(float seconds) {
            HidePickup();

            yield return new WaitForSeconds(seconds);

            ShowPickup();
        }

        private void HidePickup() {
            GetComponent<SphereCollider>().enabled = false;

            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
            }
        }

        private void ShowPickup() {
            GetComponent<SphereCollider>().enabled = true;

            foreach (Transform child in transform) {
                child.gameObject.SetActive(true);
            }
        }

        #region IRaycastable implements
        public bool HandleRaycast(PlayerController callingController) {
            if (Input.GetMouseButtonDown(1)) {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }
        #endregion
    }
}
