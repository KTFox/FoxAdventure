using UnityEngine;

namespace RPG.UI.Inventory {
    public class ShowHideUI : MonoBehaviour {

        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject[] uiContainer = null;

        private bool isShowing;

        // Start is called before the first frame update
        void Start()
        {
            Hide();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                if (isShowing)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        private void Show()
        {
            foreach (GameObject uiObject in uiContainer)
            {
                uiObject.SetActive(true);
                isShowing = true;
            }
        }

        private void Hide()
        {
            foreach (GameObject uiObject in uiContainer)
            {
                uiObject.SetActive(false);
                isShowing = false;
            }
        }
    }
}
