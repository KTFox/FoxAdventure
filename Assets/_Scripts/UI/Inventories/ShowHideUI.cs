using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ShowHideUI : MonoBehaviour
    {
        // Structs

        [System.Serializable]
        private struct uiConfig
        {
            public GameObject uiObject;
            public KeyCode toggleKey;
        }

        // Variables

        [SerializeField]
        private uiConfig[] uiConfigs;


        // Methods

        private void Start()
        {
            foreach (var uiConfig in uiConfigs)
            {
                uiConfig.uiObject.SetActive(false);
            }
        }

        private void Update()
        {
            foreach (var uiConfig in uiConfigs)
            {
                if (Input.GetKeyDown(uiConfig.toggleKey))
                {
                    uiConfig.uiObject.SetActive(!uiConfig.uiObject.activeSelf);
                }
            }
        }
    }
}
