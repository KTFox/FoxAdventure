using System.Collections;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultFileName = "KTFox_SavingFile";

        [SerializeField]
        private float fadeInTime;

        public void ContinueGame()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultFileName);

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();
            yield return fader.FadeIn(fadeInTime);
        }

        public void LoadGame()
        {
            GetComponent<SavingSystem>().Load(defaultFileName);
        }

        public void SaveGame()
        {
            GetComponent<SavingSystem>().Save(defaultFileName);
        }
    }
}
