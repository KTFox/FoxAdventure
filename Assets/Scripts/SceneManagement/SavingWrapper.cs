using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {

        private const string defaultFileName = "KTFox_SavingFile";

        [SerializeField]
        private float fadeInTime;

        private IEnumerator Start() {
            Fader fader = FindObjectOfType<Fader>();

            fader.FadeOutImmediately();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultFileName);

            yield return fader.FadeIn(fadeInTime);
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
