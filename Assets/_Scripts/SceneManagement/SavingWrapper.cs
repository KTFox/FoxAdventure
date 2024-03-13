using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string CURRENT_SAVE_FILE_NAME = "CurrentSaveFileName";

        [SerializeField]
        private float fadeInTime;
        [SerializeField]
        private int firstSceneBuildIndex = 1;

        public IEnumerable<string> SavedFileNames => GetComponent<SavingSystem>().GetSaveFileNames();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void LoadGameFromSavedFile(string savedFile)
        {
            SetCurrentSaveFileName(savedFile);
            ContinueGame();
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSaveFileName());
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSaveFileName());
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            SetCurrentSaveFileName(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSaveFileName());

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadFirstScene()
        {
            yield return SceneManager.LoadSceneAsync(firstSceneBuildIndex);

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(fadeInTime);
        }

        private void SetCurrentSaveFileName(string fileName)
        {
            PlayerPrefs.SetString(CURRENT_SAVE_FILE_NAME, fileName);
        }

        private string GetCurrentSaveFileName()
        {
            return PlayerPrefs.GetString(CURRENT_SAVE_FILE_NAME);
        }
    }
}
