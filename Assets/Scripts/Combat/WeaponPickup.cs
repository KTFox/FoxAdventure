using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour {

        [SerializeField]
        private WeaponSO weaponSO;
        [SerializeField]
        private float hideTime;

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<Fighter>().EquipWeapon(weaponSO);
                StartCoroutine(HideForSeconds(hideTime));
            }
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
    }
}
