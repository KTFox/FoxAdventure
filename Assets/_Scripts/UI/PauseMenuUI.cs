using UnityEngine;
using RPG.Control;
using RPG.SceneManagement;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (_playerController == null) return;

            Time.timeScale = 0f;
            _playerController.enabled = false;
        }

        private void OnDisable()
        {
            if (_playerController == null) return;

            Time.timeScale = 1f;
            _playerController.enabled = true;
        }

        #region Unity events
        public void Save()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();
        }

        public void SaveAndQuit()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();
            savingWrapper.LoadMenuScene();
        }
        #endregion
    }
}
