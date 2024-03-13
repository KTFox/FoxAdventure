using UnityEngine;

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
        }
    }
}
