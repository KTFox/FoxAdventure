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
        private float fadeInTime = 0.2f;
        [SerializeField]
        private float fadeOutTime = 0.2f;

        private int menuSceneBuildIndex = 0;
        private int firstLevelBuildIndex = 1;

        public IEnumerable<string> SavedFileNames => GetComponent<SavingSystem>().GetSaveFileNames();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveData();
            }
        }

        public void LoadMenuGame()
        {
            StartCoroutine(LoadMenuScene());
        }

        public void LoadGameFromSavedFile(string savedFile)
        {
            SetCurrentSaveFileName(savedFile);
            ContinueGame();
        }

        public void LoadData()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSaveFileName());
        }

        public void SaveData()
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

        private IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSaveFileName());

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadFirstScene()
        {
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuSceneBuildIndex);
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
