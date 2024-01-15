using UnityEngine;

namespace RPG.Saving {
    public class SavingWrapper : MonoBehaviour {

        private const string fileName = "KTFox_SavingFile";

        private SavingSystem savingSystem;

        private void Start() {
            savingSystem = GetComponent<SavingSystem>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                savingSystem.Save(fileName);
            }
            if (Input.GetKeyUp(KeyCode.L)) {
                savingSystem.Load(fileName);
            }
        }
    }
}
