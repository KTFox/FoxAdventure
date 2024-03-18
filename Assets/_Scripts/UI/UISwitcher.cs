using UnityEngine;

namespace RPG.UI
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField]
        private GameObject _entryUI;

        private void Start()
        {
            SwitchTo(_entryUI);
        }

        public void SwitchTo(GameObject uiToDisplay)
        {
            if (uiToDisplay.transform.parent != transform) return;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == uiToDisplay);
            }
        }
    }
}
