using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        // Variables

        private const string CURRENT_SAVE_FILE_NAME = "CurrentSaveFileName";

        [SerializeField]
        private float _fadeTime = 0.2f;

        private int _menuSceneBuildIndex = 0;
        private int _firstLevelBuildIndex = 1;

        // Properties

        public IEnumerable<string> SavedFileNames => GetComponent<SavingSystem>().GetSaveFileNames();



        // Methods

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveData();
            }
        }

        public void LoadMenuScene()
        {
            StartCoroutine(LoadMenuSceneCoroutine());
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
            StartCoroutine(LoadLastSceneCoroutine());
        }

        public void CreateNewGame(string saveFile)
        {
            SetCurrentSaveFileName(saveFile);
            StartCoroutine(LoadFirstSceneCoroutine());
        }

        private IEnumerator LoadLastSceneCoroutine()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSaveFileName());

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(_fadeTime);
        }

        private IEnumerator LoadFirstSceneCoroutine()
        {
            yield return SceneManager.LoadSceneAsync(_firstLevelBuildIndex);

            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediately();

            yield return fader.FadeIn(_fadeTime);
        }

        private IEnumerator LoadMenuSceneCoroutine()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(_fadeTime);
            yield return SceneManager.LoadSceneAsync(_menuSceneBuildIndex);
            yield return fader.FadeIn(_fadeTime);
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
