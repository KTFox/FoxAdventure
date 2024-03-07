using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.EffectStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/EffectStrategySO/SpawnTargetPrefabEffectSO")]
    public class SpawnTargetPrefabEffectSO : EffectStrategySO
    {
        [SerializeField]
        private Transform prefabToSpawn;
        [SerializeField]
        private float timeToDestroy;

        public override void StartEffect(AbilityData data, Action finishEffect)
        {
            data.StartCoroutine(SpawnPrefab(data, finishEffect));
        }

        IEnumerator SpawnPrefab(AbilityData data, Action finishEffect)
        {
            Transform instance = Instantiate(prefabToSpawn);
            instance.position = data.TargetPoint;

            if (timeToDestroy > 0)
            {
                yield return new WaitForSeconds(timeToDestroy);

                Destroy(instance.gameObject);
            }

            finishEffect();
        }
    }
}
