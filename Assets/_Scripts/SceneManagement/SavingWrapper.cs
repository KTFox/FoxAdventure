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
        private float _fadeTime;

        private int _menuSceneBuildIndex = 0;
        private int _firstLevelBuildIndex = 1;

        // Properties

        public IEnumerable<string> SavedFileNames => GetComponent<SavingSystem>().GetSaveFileNames();


        // Methods

        /// <summary>
        /// Create new game and set the name of saving file equal fileName
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateNewGame(string fileName)
        {
            SetCurrentSaveFileName(fileName);
            StartCoroutine(LoadFirstSceneCoroutine());
        }

        #region ContinueGame overloads
        public void ContinueGame(string fileName)
        {
            SetCurrentSaveFileName(fileName);
            ContinueGame();
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadLastSceneCoroutine());
        }
        #endregion

        public void LoadMenuScene()
        {
            StartCoroutine(LoadMenuSceneCoroutine());
        }

        /// <summary>
        /// Restore game's state from current saved file
        /// </summary>
        public void RestoreGameState()
        {
            GetComponent<SavingSystem>().RestoreGameStateFromFile(GetCurrentSaveFileName());
        }

        /// <summary>
        /// Save game's state into current saved file
        /// </summary>
        public void SaveGameState()
        {
            GetComponent<SavingSystem>().SaveGameStateIntoFile(GetCurrentSaveFileName());
        }

        private IEnumerator LoadFirstSceneCoroutine()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeTime);
            yield return SceneManager.LoadSceneAsync(_firstLevelBuildIndex);
            yield return fader.FadeIn(_fadeTime);
        }

        private IEnumerator LoadLastSceneCoroutine()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeTime);
            yield return GetComponent<SavingSystem>().LoadLastSceneFromFile(GetCurrentSaveFileName());
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
