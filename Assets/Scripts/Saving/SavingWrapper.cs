using UnityEngine;

namespace RPG.Saving {
    public class SavingWrapper : MonoBehaviour {

        private const string fileName = "KTFox_SavingFile";

        private void Start() {
            LoadGame();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                SaveGame();
            }
            if (Input.GetKeyUp(KeyCode.L)) {
                LoadGame();
            }
        }

        public void LoadGame() {
            GetComponent<SavingSystem>().Load(fileName);
        }

        public void SaveGame() {
            GetComponent<SavingSystem>().Save(fileName);
        }
    }
}
