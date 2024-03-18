using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Health _healthComponent;
        [SerializeField]
        private Transform _healthVisual;
        [SerializeField]
        private Canvas _rootCanvas;


        // Methods

        private void Update()
        {
            if (Mathf.Approximately(_healthComponent.CurrentHealthFraction, 1f) || Mathf.Approximately(_healthComponent.CurrentHealthFraction, 0f))
            {
                _rootCanvas.enabled = false;
                return;
            }

            _rootCanvas.enabled = true;
            _healthVisual.localScale = new Vector3(_healthComponent.CurrentHealthFraction, 1f, 1f);
        }
    }
}
