using System;
using UnityEngine;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/SpawnProjectileEffectSO")]
    public class SpawnProjectileEffectSO : EffectStrategySO
    {
        [SerializeField]
        private Projectile projectileToSpawn;
        [SerializeField]
        private float damage;
        [SerializeField]
        private bool isRightHand;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            Fighter fighter = data.User.GetComponent<Fighter>();
            foreach (GameObject target in data.Targets)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = fighter.GetHandTransform(isRightHand);
                    projectile.SetTarget(targetHealth, data.User, damage);
                }
            }
        }
    }
}
