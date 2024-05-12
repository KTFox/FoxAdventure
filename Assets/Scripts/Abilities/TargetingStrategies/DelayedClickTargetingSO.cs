using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using RPG.Control;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/TargetingStrategy/DelayedClickTargeting")]
    public class DelayedClickTargetingSO : TargetingStrategySO
    {
        // Variables 

        [SerializeField]
        private Texture2D _cursorTexture;
        [SerializeField]
        private Vector3 _cursorHotSpot;
        [SerializeField]
        private LayerMask _affectedLayerMask;
        [SerializeField]
        private float _areaAffectRadius;
        [SerializeField]
        private Transform _targetingVisualPrefab;
        private Transform targetingVisualInstance;


        //Methods

        public override void StartTargeting(AbilityData abilityData, Action finishedCallback)
        {
            var playerController = abilityData.Instigator.GetComponent<PlayerController>();
            playerController.StartCoroutine(TargetingCoroutine(abilityData, playerController, finishedCallback));
        }

        private IEnumerator TargetingCoroutine(AbilityData abilityData, PlayerController playerController, Action finishedCallback)
        {
            playerController.enabled = false;

            // Set active targeting visual
            if (targetingVisualInstance == null)
            {
                targetingVisualInstance = Instantiate(_targetingVisualPrefab);
            }
            else
            {
                targetingVisualInstance.gameObject.SetActive(true);
            }
            targetingVisualInstance.localScale = new Vector3(_areaAffectRadius * 2, 1, _areaAffectRadius * 2);

            while (!abilityData.IsCancelled)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, _affectedLayerMask))
                {
                    targetingVisualInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButtonDown(0));

                        abilityData.TargetPoint = raycastHit.point;
                        abilityData.Targets = GetGameObjectsInRadius(raycastHit.point);

                        break;
                    }
                }

                yield return null;
            }

            playerController.enabled = true;
            targetingVisualInstance.gameObject.SetActive(false);
            finishedCallback();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 centerPoint)
        {
            RaycastHit[] hits = Physics.SphereCastAll(centerPoint, _areaAffectRadius, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}
