using UnityEngine;
using RPG.Control;
using RPG.SceneManagement;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (playerController == null) return;

            Time.timeScale = 0f;
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            if (playerController == null) return;

            Time.timeScale = 1f;
            playerController.enabled = true;
        }

        #region Unity events
        public void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();
            savingWrapper.LoadMenuGame();
        }
        #endregion
    }
}
