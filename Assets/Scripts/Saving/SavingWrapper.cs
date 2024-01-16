using System.Collections;
using UnityEngine;

namespace RPG.Saving {
    public class SavingWrapper : MonoBehaviour {

        private const string defaultFileName = "KTFox_SavingFile";

        private IEnumerator Start() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultFileName);
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
            GetComponent<SavingSystem>().Load(defaultFileName);
        }

        public void SaveGame() {
            GetComponent<SavingSystem>().Save(defaultFileName);
        }
    }
}
