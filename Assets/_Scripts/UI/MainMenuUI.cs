using UnityEngine;
using TMPro;
using RPG.SceneManagement;
using RPG.Utility;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TMP_InputField _inputField;
        private LazyValue<SavingWrapper> savingWrapper;


        // Methods

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        #region Unity events
        public void ContinueGame()
        {
            savingWrapper.Value.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.Value.CreateNewGame(_inputField.text);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}
