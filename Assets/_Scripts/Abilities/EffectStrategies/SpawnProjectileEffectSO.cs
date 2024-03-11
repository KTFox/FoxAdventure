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
        [SerializeField]
        private bool useTargetPoint;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            Fighter fighter = data.User.GetComponent<Fighter>();
            Vector3 spawnPosition = fighter.GetHandTransform(isRightHand);

            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                SpawnProjectileForTargets(data, spawnPosition);
            }

            finishEffect();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(data.User, damage, data.TargetPoint);
        }

        private void SpawnProjectileForTargets(AbilityData data, Vector3 spawnPosition)
        {
            foreach (GameObject target in data.Targets)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth)
                {
                    Projectile projectile = Instantiate(projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(data.User, damage, targetHealth);
                }
            }
        }
    }
}
