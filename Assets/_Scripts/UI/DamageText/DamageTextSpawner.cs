using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText _damageTextPrefab;

        #region Unity Events
        public void SpawnDamageText(float damageAmount)
        {
            DamageText damageText = Instantiate(_damageTextPrefab, transform);
            damageText.UpdateText(damageAmount);
        }
        #endregion
    }
}
