using System.Collections;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {

        [SerializeField] 
        private WeaponSO weaponSO;
        [SerializeField] 
        private float healthRestoreAmount;
        [SerializeField] 
        private float hideTime;

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player")) {
                Pickup(collision.gameObject);
            }
        }

        private void Pickup(GameObject subject) {
            if (weaponSO != null) {
                subject.GetComponent<Fighter>().EquipWeapon(weaponSO);
            }
            if (healthRestoreAmount > 0) {
                subject.GetComponent<Health>().Heal(healthRestoreAmount);
            }
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
                Pickup(callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }
        #endregion
    }
}
