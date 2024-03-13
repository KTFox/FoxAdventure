using UnityEngine;
using RPG.SceneManagement;
using RPG.Utility;
using System;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
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
        #endregion
    }
}
