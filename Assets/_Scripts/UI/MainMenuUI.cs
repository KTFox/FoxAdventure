using UnityEngine;
using TMPro;
using RPG.SceneManagement;
using RPG.Utility;
using UnityEngine.UI;
using System.Linq;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Button _continueButton;
        [SerializeField]
        private TMP_InputField _inputField;

        private LazyValue<SavingWrapper> _savingWrapper;


        // Methods

        private void Awake()
        {
            _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private void Start()
        {
            _continueButton.interactable = _savingWrapper.Value.SavedFileNames.Count() > 0;
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        #region Unity events
        public void ContinueGame()
        {
            _savingWrapper.Value.ContinueGame();
        }

        public void NewGame()
        {
            _savingWrapper.Value.CreateNewGame(_inputField.text);
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
