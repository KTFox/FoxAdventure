using UnityEngine;

namespace RPG.UI {
    public class ShowHideUI : MonoBehaviour {

        [SerializeField]
        private GameObject uiContainer;

        private void Start() {
            uiContainer.SetActive(false);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.I)) {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }

    }
}
