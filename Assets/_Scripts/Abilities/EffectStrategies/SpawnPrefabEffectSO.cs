using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/EffectStrategy/SpawnTargetPrefabEffect")]
    public class SpawnPrefabEffectSO : EffectStrategySO
    {
        // Variables

        [SerializeField]
        private Transform _prefabToSpawn;
        [SerializeField]
        private float _timeToBeDestroyed;


        // Methods

        public override void StartEffect(AbilityData abilityData, Action finishedCallback)
        {
            abilityData.StartCoroutine(SpawnPrefabCoroutine(abilityData, finishedCallback));
        }

        private IEnumerator SpawnPrefabCoroutine(AbilityData abilityData, Action finishedCallback)
        {
            Transform spawnedPrefab = Instantiate(_prefabToSpawn);
            spawnedPrefab.position = abilityData.TargetPoint;

            if (_timeToBeDestroyed > 0)
            {
                yield return new WaitForSeconds(_timeToBeDestroyed);

                Destroy(spawnedPrefab.gameObject);
            }

            finishedCallback();
        }
    }
}
