using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _damageText;

        public void UpdateText(float damageAmount)
        {
            _damageText.text = $"{damageAmount:N2}";
        }

        #region Animation events
        public void DestroyText()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
