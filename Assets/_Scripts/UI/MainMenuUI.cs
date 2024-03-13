using UnityEngine;
using TMPro;
using RPG.SceneManagement;
using RPG.Utility;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField inputField;

        private LazyValue<SavingWrapper> savingWrapper;

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
            savingWrapper.Value.NewGame(inputField.text);
        }
        #endregion
    }
}
