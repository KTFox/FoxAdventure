using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement {
    public class SavingWrapper : MonoBehaviour {

        private const string defaultFileName = "KTFox_SavingFile";

        [SerializeField] 
        private float fadeInTime;

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultFileName);

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.S)) {
                SaveGame();
            } else if (Input.GetKeyDown(KeyCode.L)) {
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
