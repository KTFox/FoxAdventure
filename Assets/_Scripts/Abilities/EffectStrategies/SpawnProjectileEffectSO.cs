using System;
using UnityEngine;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/SpawnProjectileEffect")]
    public class SpawnProjectileEffectSO : EffectStrategySO
    {
        // Variables

        [SerializeField]
        private Projectile _projectileToSpawn;
        [SerializeField]
        private float _damage;
        [SerializeField]
        private bool _isRightHand;
        [SerializeField]
        private bool _useTargetPoint;


        // Methods

        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            var fighter = abilityData.Instigator.GetComponent<Fighter>();
            var projectileSpawnPosition = fighter.GetHandPosition(_isRightHand);

            if (_useTargetPoint)
            {
                SpawnProjectileForTargetPoint(abilityData, projectileSpawnPosition);
            }
            else
            {
                SpawnProjectileForTargets(abilityData, projectileSpawnPosition);
            }

            finishedCallback();
        }

        private void SpawnProjectileForTargetPoint(AbilityData abilityData, Vector3 spawnPosition)
        {
            Projectile projectile = Instantiate(_projectileToSpawn);
            projectile.transform.position = spawnPosition;
            projectile.SetTarget(abilityData.Instigator, _damage, abilityData.TargetPoint);
        }

        private void SpawnProjectileForTargets(AbilityData abilityData, Vector3 spawnPosition)
        {
            foreach (GameObject target in abilityData.Targets)
            {
                Health targetHealth = target.GetComponent<Health>();
                if (targetHealth)
                {
                    Projectile projectile = Instantiate(_projectileToSpawn);
                    projectile.transform.position = spawnPosition;
                    projectile.SetTarget(abilityData.Instigator, _damage, targetHealth);
                }
            }
        }
    }
}
