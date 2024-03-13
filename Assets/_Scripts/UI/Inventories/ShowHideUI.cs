using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField]
        private uiConfig[] uiConfigs;

        [System.Serializable]
        private struct uiConfig
        {
            public GameObject uiObject;
            public KeyCode toggleKey;
        }

        private void Start()
        {
            foreach (var config in uiConfigs)
            {
                config.uiObject.SetActive(false);
            }
        }

        private void Update()
        {
            foreach (var config in uiConfigs)
            {
                if (Input.GetKeyDown(config.toggleKey))
                {
                    config.uiObject.SetActive(!config.uiObject.activeSelf);
                }
            }
        }
    }
}
