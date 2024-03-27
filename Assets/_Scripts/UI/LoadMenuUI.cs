using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.SceneManagement;

namespace RPG.UI
{
    public class LoadMenuUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Transform _rootTransform;
        [SerializeField]
        private GameObject _buttonPrefab;


        // Methods

        private void OnEnable()
        {
            foreach (Transform child in _rootTransform)
            {
                Destroy(child.gameObject);
            }

            var savingWrapper = FindObjectOfType<SavingWrapper>();

            if (savingWrapper == null) return;

            foreach (string savedFileName in savingWrapper.SavedFileNames)
            {
                GameObject buttonObject = Instantiate(_buttonPrefab, _rootTransform);
                TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
                Button button = buttonObject.GetComponent<Button>();

                buttonText.text = savedFileName;
                button.onClick.AddListener(() => savingWrapper.ContinueGame(savedFileName));
            }
        }
    }
}
