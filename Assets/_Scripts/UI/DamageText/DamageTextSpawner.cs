using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField]
        private DamageText damageTextPrefab;

        #region Unity Events
        public void SpawnDamageText(float damageAmount)
        {
            DamageText damageText = Instantiate(damageTextPrefab, transform);
            damageText.UpdateText(damageAmount);
        }
        #endregion
    }
}
