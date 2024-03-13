using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.SceneManagement;

namespace RPG.UI
{
    public class LoadMenuUI : MonoBehaviour
    {
        [SerializeField]
        private Transform rootTransform;
        [SerializeField]
        private GameObject buttonPrefab;

        private void OnEnable()
        {
            foreach (Transform child in rootTransform)
            {
                Destroy(child.gameObject);
            }

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) return;
            foreach (string savedFileName in savingWrapper.SavedFileNames)
            {
                GameObject buttonObject = Instantiate(buttonPrefab, rootTransform);
                TextMeshProUGUI buttonText = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
                Button button = buttonObject.GetComponent<Button>();

                buttonText.text = savedFileName;
                button.onClick.AddListener(() => savingWrapper.LoadGameFromSavedFile(savedFileName));
            }
        }
    }
}
