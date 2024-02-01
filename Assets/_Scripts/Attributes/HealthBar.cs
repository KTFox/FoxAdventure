using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour {

        [SerializeField] 
        private Health characterHealth;
        [SerializeField] 
        private Transform foreground;
        [SerializeField] 
        private Canvas rootCanvas;

        private void Update() {
            if (Mathf.Approximately(characterHealth.CurrentHealthFraction, 1f) || Mathf.Approximately(characterHealth.CurrentHealthFraction, 0f)) {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(characterHealth.CurrentHealthFraction, 1f, 1f);
        }
    }
}
